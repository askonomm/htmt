using System.Text.RegularExpressions;
using System.Xml;
using Htmt.AttributeParsers;

namespace Htmt;

public partial class Parser
{
    public XmlDocument Xml { get; } = new();

    public string Template { get; set; } = string.Empty;

    public Dictionary<string, object?> Data { get; init; } = new();

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
            
            RemoveDoctype();
            CloseVoidElements();
        }
        
        var templateStr = $"<root xmlns:x=\"{HtmtNamespace}\">{Template}</root>";
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
            new IfAttributeParser(),
            new UnlessAttributeParser(),
            new InnerPartialAttributeParser(),
            new OuterPartialAttributeParser(),
            new GenericValueAttributeParser(),
        ];
    }
    
    /**
     * Detects if the template is an HTML document.
     */
    private static bool IsHtml(string template)
    {
        var doctype = template.Trim().StartsWith("<!DOCTYPE", StringComparison.OrdinalIgnoreCase);
        var htmlTag = template.Trim().StartsWith("<html", StringComparison.OrdinalIgnoreCase);
        
        return doctype || htmlTag;
    }
    
    [GeneratedRegex(@"<!DOCTYPE[^>]*>", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex DocTypeRegex();
    
    /**
     * Removes the doctype from the template to avoid issues with the XML parser.
     */
    private void RemoveDoctype()
    {
        Template = DocTypeRegex().Replace(Template, string.Empty);
    }
    
    /**
     * Gets the doctype from the template, so it can be added back to the final HTML.
     */
    private static string GetDoctype(string template)
    {
        var match = DocTypeRegex().Match(template);
        
        return match.Success ? match.Value : string.Empty;
    }
    
    private void CloseVoidElements()
    {
        var voidElements = new[]
        {
            "area", 
            "base", 
            "br", 
            "col", 
            "embed",
            "hr", 
            "img", 
            "input", 
            "link", 
            "meta", 
            "param", 
            "source", 
            "track", 
            "wbr"
        };
        
        var regex = new Regex(@"(?<el><(" + string.Join('|', voidElements) + @")([^>]*?)>)", RegexOptions.IgnoreCase);
    
        foreach(Match match in regex.Matches(Template))
        {
            var element = match.Groups["el"].Value;
            
            // Already closed, skip
            if (element.EndsWith("/>")) continue;
            
            // replace with self-closing tag
            var newElement = element.Insert(element.Length - 1, "/");
            Template = Template.Replace(element, newElement);
        }
    }
    
    /**
     * Parses the template and returns it as HTML.
     */
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
    
    /**
     * Parses the template and returns it as XML.
     */
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
            var nodes = Xml.DocumentElement?.SelectNodes(parser.XTag, _nsManager);
            var clonedData = new Dictionary<string, object?>(Data);
            parser.Parse(Xml, clonedData, nodes);
        }
        
        // Remove all leftover attributes that start with x:
        var leftOverNodes = Xml.DocumentElement?.SelectNodes("//*[@*[starts-with(name(), 'x:')]]", _nsManager);
        
        if (leftOverNodes == null) return;
        
        foreach (var node in leftOverNodes.Cast<XmlNode>())
        {
            if (node is not XmlElement element) continue;
                
            var attributes = element.Attributes.Cast<XmlAttribute>()
                .Where(a => a.Name.StartsWith("x:"))
                .ToList();
                
            foreach (var attr in attributes)
            {
                element.RemoveAttribute(attr.Name);
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