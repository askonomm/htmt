using System.Text.RegularExpressions;
using System.Xml;
using Htmt.AttributeParsers;

namespace Htmt;

public class Parser
{
    public XmlDocument Xml { get; } = new();

    public string Template { get; init; } = string.Empty;

    public Dictionary<string, object> Data { get; init; } = new();

    public IAttributeParser[] AttributeParsers { get; init; } = [];
    
    private XmlNamespaceManager _nsManager = null!;
    
    private bool _isHtml;
    
    private string _docType = string.Empty;
    
    private readonly XmlReaderSettings _xmlSettings = new()
    {
        IgnoreWhitespace = true,
        IgnoreComments = true,
        DtdProcessing = DtdProcessing.Ignore,
        ValidationType = ValidationType.None,
        XmlResolver = null
    };
    
    private const string HtmtNamespace = "http://www.w3.org/1999/xhtml";

    private void Parse()
    {
        _nsManager = new XmlNamespaceManager(Xml.NameTable);
        _nsManager.AddNamespace("x", HtmtNamespace);
        
        if (IsHtml(Template))
        {
            _isHtml = true;
            _docType = GetDoctype(Template);
        }
        
        var templateWithoutDoctype = RemoveDoctype(Template);
        var templateStr = $"<root xmlns:x=\"{HtmtNamespace}\">{templateWithoutDoctype}</root>";
        using var reader = XmlReader.Create(new StringReader(templateStr), _xmlSettings);
        Xml.Load(reader);
        
        AddIdentifierToNodes();
        RunAttributeParsers();
        RemoveIdentifierFromNodes();
    }

    public static IAttributeParser[] DefaultAttributeParsers()
    {
        return
        [
            new ForAttributeParser(),
            new InnerTextAttributeParser(),
            new OuterTextAttributeParser(),
            new InnerHtmlAttributeParser(),
            new OuterHtmlAttributeParser(),
            new HrefAttributeParser(),
            new IfAttributeParser(),
            new UnlessAttributeParser(),
        ];
    }
    
    private static bool IsHtml(string template)
    {
        // Any document that start with <!DOCTYPE*
        return template.Trim().StartsWith("<!DOCTYPE", StringComparison.OrdinalIgnoreCase);
    }
    
    private static string RemoveDoctype(string template)
    {
        var doctypeRegex = new Regex(@"<!DOCTYPE[^>]*>", RegexOptions.IgnoreCase);
        return doctypeRegex.Replace(template, string.Empty);
    }
    
    private static string GetDoctype(string template)
    {
        var doctypeRegex = new Regex(@"<!DOCTYPE[^>]*>", RegexOptions.IgnoreCase);
        var match = doctypeRegex.Match(template);
        
        return match.Success ? match.Value : string.Empty;
    }
    
    public string ToHtml()
    {
        Parse();
        
        if (Xml.DocumentElement == null) return string.Empty;
        
        if (_isHtml)
        {
            return $"{_docType}{Xml.DocumentElement.FirstChild?.OuterXml}";
        }
        
        return Xml.DocumentElement.FirstChild?.OuterXml ?? string.Empty;
    }
    
    public XmlNode ToXml()
    {
        Parse();
        
        return Xml.DocumentElement?.FirstChild ?? Xml.CreateElement("root");
    }
    
    private void RunAttributeParsers()
    {
        var parsers = AttributeParsers;
        
        if (parsers.Length == 0)
        {
            parsers = DefaultAttributeParsers();
        }
        
        foreach(var parser in parsers)
        {
            var nodes = Xml.DocumentElement?.SelectNodes($"//*[@x:{parser.Name}]", _nsManager);
            var clonedData = new Dictionary<string, object>(Data);
            parser.Parse(Xml, clonedData, nodes);
            
            // Clean up named attributes
            if (nodes == null) continue;
            
            foreach (var node in nodes)
            {
                if (node is not XmlElement n) continue;
                
                n.RemoveAttribute($"x:{parser.Name}");
            }
        }
    }

    private void AddIdentifierToNodes()
    {
        if (Xml.DocumentElement == null) return;
        
        var nodesToProcess = new Queue<XmlNode>(Xml.DocumentElement.ChildNodes.Cast<XmlNode>());

        while (nodesToProcess.Count > 0)
        {
            var node = nodesToProcess.Dequeue();
            if (node is not XmlElement element) continue;

            var uuid = Guid.NewGuid().ToString();
            element.SetAttribute("data-htmt-id", uuid);

            foreach (XmlNode child in element.ChildNodes)
            {
                nodesToProcess.Enqueue(child);
            }
        }
    }

    private void RemoveIdentifierFromNodes()
    {
        // null check
        if (Xml.DocumentElement == null) return;

        var nodesToProcess = new Queue<XmlNode>(Xml.DocumentElement.ChildNodes.Cast<XmlNode>());

        while (nodesToProcess.Count > 0)
        {
            var node = nodesToProcess.Dequeue();
            if (node is not XmlElement element) continue;

            element.RemoveAttribute("data-htmt-id");

            foreach (XmlNode child in element.ChildNodes)
            {
                nodesToProcess.Enqueue(child);
            }
        }
    }
}