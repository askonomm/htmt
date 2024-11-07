using System.Text.RegularExpressions;
using System.Xml;
using Htmt.AttributeParsers;
using Htmt.ExpressionModifiers;

namespace Htmt;

/// <summary>
/// The main parser class that parses the template and returns it as HTML or XML.
/// </summary>
public partial class Parser
{
    /// <summary>
    /// The XML document that is being parsed.
    /// </summary>
    private XmlDocument Xml { get; } = new();

    /// <summary>
    /// The template to parse.
    /// </summary>
    public required string Template { get; set; }

    /// <summary>
    /// The data that the parser uses to find values.
    /// </summary>
    public Dictionary<string, object?> Data { get; init; } = [];

    /// <summary>
    /// The attribute parsers that the parser uses to parse attributes.
    /// </summary>
    private IAttributeParser[] AttributeParsers { get; init; } = DefaultAttributeParsers();

    /// <summary>
    /// The expression modifiers that the parser uses to modify expressions.
    /// </summary>
    private IExpressionModifier[] ExpressionModifiers { get; init; } = DefaultExpressionModifiers();

    /// <summary>
    /// The namespace manager for the XML document.
    /// </summary>
    private XmlNamespaceManager _nsManager = null!;

    /// <summary>
    /// The doctype of the document.
    /// </summary>
    private string _docType = string.Empty;

    /// <summary>
    /// The settings for the XML reader.
    /// </summary>
    private readonly XmlReaderSettings _xmlSettings = new()
    {
        IgnoreWhitespace = true,
        IgnoreComments = true,
        DtdProcessing = DtdProcessing.Ignore,
        ValidationType = ValidationType.None,
        XmlResolver = null
    };

    /// <summary>
    /// Htmt uses the XHTML namespace for parsing HTML documents.
    /// </summary>
    private const string HtmtNamespace = "http://www.w3.org/1999/xhtml";

    /// <summary>
    /// The constructor for the parser.
    /// </summary>
    private void Parse()
    {
        _nsManager = new XmlNamespaceManager(Xml.NameTable);
        _nsManager.AddNamespace("x", HtmtNamespace);
        _docType = GetDoctype(Template);

        RemoveDoctype();
        FillBooleanAttributes();
        CloseVoidElements();
        TransformHtmlEntities();

        var templateStr = $"<root xmlns:x=\"{HtmtNamespace}\">{Template}</root>";
        using var reader = XmlReader.Create(new StringReader(templateStr), _xmlSettings);
        Xml.Load(reader);

        AddIdentifierToNodes();
        RunAttributeParsers();
        RemoveIdentifierFromNodes();
        RemoveXNamespace();
        RemoveXmlnsFromChildren();
    }

    /// <summary>
    /// Default attribute parsers.
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// Default expression modifiers.
    /// </summary>
    /// <returns></returns>
    public static IExpressionModifier[] DefaultExpressionModifiers()
    {
        return
        [
            new DateExpressionModifier(),
            new CapitalizeExpressionModifier(),
            new LowercaseExpressionModifier(),
            new UppercaseExpressionModifier(),
            new TruncateExpressionModifier(),
            new ReverseExpressionModifier(),
            new CountExpressionModifier(),
        ];
    }

    /// <summary>
    /// The regex for the doc declaration (HTML or XML).
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"(<!DOCTYPE[^>]*>)|(<\?xml[^>]*\?>)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex DocTypeRegex();

    /// <summary>
    /// Removes the doctype from the template.
    /// </summary>
    private void RemoveDoctype()
    {
        Template = DocTypeRegex().Replace(Template, string.Empty);
    }
    
    /// <summary>
    /// Gets the doctype from the template.
    /// </summary>
    /// <param name="template">The template to get the doctype from.</param>
    /// <returns>Returns the doctype.</returns>
    private static string GetDoctype(string template)
    {
        var match = DocTypeRegex().Match(template);

        return match.Success ? match.Value : string.Empty;
    }

    /// <summary>
    /// Replaces void elements with self-closing tags.
    /// This is necessary because the XML parser does not handle void elements.
    /// </summary>
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

        foreach (Match match in regex.Matches(Template))
        {
            var element = match.Groups["el"].Value;

            // Already closed, skip
            if (element.EndsWith("/>")) continue;

            // replace with self-closing tag
            var newElement = element.Insert(element.Length - 1, "/");
            Template = Template.Replace(element, newElement);
        }
    }
    
    [GeneratedRegex(@"(<[^>]*?)(?<attr>\s[\w-]+)(?=\s|>|/>)", RegexOptions.IgnoreCase)]
    private static partial Regex BooleanAttributeRegex(); 

    /// <summary>
    /// Fills boolean HTML attributes such as "checked" or "defer".
    /// </summary>
    private void FillBooleanAttributes()
    {
        var voidAttributes = new Dictionary<string, string>
        {
            { "allowfullscreen", "" },
            { "async", "" },
            { "autofocus", "" },
            { "autoplay", "" },
            { "checked", "" },
            { "controls", "" },
            { "default", "" },
            { "defer", "" },
            { "disabled", "" },
            { "formnovalidate", "" },
            { "hidden", "" },
            { "ismap", "" },
            { "itemscope", "" },
            { "loop", "" },
            { "multiple", "" },
            { "muted", "" },
            { "nomodule", "" },
            { "novalidate", "" },
            { "open", "" },
            { "playsinline", "" },
            { "readonly", "" },
            { "required", "" },
            { "reversed", "" },
            { "selected", "" }
        };
        
        foreach (Match match in BooleanAttributeRegex().Matches(Template))
        {
            var attribute = match.Groups["attr"].Value.Trim();

            if (!voidAttributes.TryGetValue(attribute, out var val)) continue;

            // Replace the attribute with the filled one
            Template = Template.Replace(attribute, $"{attribute}=\"{val}\"");
        }
    }

    /// <summary>
    /// The regex for HTML entities.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"&(?<entity>\w+);")]
    private static partial Regex HtmlEntityRegex();

    /// <summary>
    /// Transforms HTML entities to their respective characters.
    /// This is necessary because the XML parser does not handle HTML entities.
    /// </summary>
    private void TransformHtmlEntities()
    {
        var entityRegex = HtmlEntityRegex();
        var matches = entityRegex.Matches(Template);

        foreach (Match match in matches)
        {
            var entity = match.Groups["entity"].Value;
            var replacement = System.Net.WebUtility.HtmlDecode($"&{entity};");

            Template = Template.Replace(match.Value, replacement);
        }
    }

    /// <summary>
    /// Returns the parsed template as HTML.
    /// </summary>
    /// <returns></returns>
    public string ToHtml()
    {
        Parse();

        return Xml.DocumentElement == null ? string.Empty : $"{_docType}{Xml.DocumentElement.InnerXml}";
    }

    /// <summary>
    /// Returns the parsed template as XML.
    /// </summary>
    /// <returns></returns>
    public XmlNode ToXml()
    {
        Parse();

        return Xml.DocumentElement?.FirstChild ?? Xml.CreateElement("root");
    }

    /// <summary>
    /// Runs all attribute parsers.
    /// </summary>
    private void RunAttributeParsers()
    {
        foreach (var parser in AttributeParsers)
        {
            parser.Xml = Xml;
            parser.Data = new Dictionary<string, object?>(Data);;
            parser.ExpressionModifiers = ExpressionModifiers;
            
            var nodes = Xml.DocumentElement?.SelectNodes(parser.XTag, _nsManager);
            
            parser.Parse(nodes);
        }

        // Remove all leftover attributes that start with x: or data-htmt-:
        var leftOverNodes = Xml.DocumentElement?.SelectNodes("//*[@*[starts-with(name(), 'x:') or starts-with(name(), 'data-htmt-')]]", _nsManager);

        if (leftOverNodes == null) return;

        foreach (var node in leftOverNodes.Cast<XmlNode>())
        {
            if (node is not XmlElement element) continue;

            var attributes = element.Attributes.Cast<XmlAttribute>()
                .Where(a => a.Name.StartsWith("x:") || a.Name.StartsWith("data-htmt-"))
                .ToList();

            foreach (var attr in attributes)
            {
                element.RemoveAttribute(attr.Name);
            }
        }   
    }

    /// <summary>
    /// Adds a unique identifier to all nodes.
    /// </summary>
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

    /// <summary>
    /// Removes the unique identifier from all nodes.
    /// </summary>
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

    /// <summary>
    /// Removes the x namespace from the document.
    /// </summary>
    private void RemoveXNamespace()
    {
        if (Xml.DocumentElement == null) return;

        var nodesToProcess = new Queue<XmlNode>(Xml.DocumentElement.ChildNodes.Cast<XmlNode>());

        while (nodesToProcess.Count > 0)
        {
            var node = nodesToProcess.Dequeue();
            if (node is not XmlElement element) continue;

            element.RemoveAttribute("xmlns:x");

            foreach (XmlNode child in element.ChildNodes)
            {
                nodesToProcess.Enqueue(child);
            }
        }
    }

    /// <summary>
    /// Removes the xmlns attribute from all children.
    /// </summary>
    private void RemoveXmlnsFromChildren()
    {
        if (Xml.DocumentElement == null) return;

        var nodesToProcess = new Queue<XmlNode>(Xml.DocumentElement.ChildNodes.Cast<XmlNode>());

        while (nodesToProcess.Count > 0)
        {
            var node = nodesToProcess.Dequeue();
            if (node is not XmlElement element) continue;

            foreach (XmlNode child in element.ChildNodes)
            {
                if (child is XmlElement childElement)
                {
                    childElement.RemoveAttribute("xmlns");
                }

                nodesToProcess.Enqueue(child);
            }
        }
    }
}
