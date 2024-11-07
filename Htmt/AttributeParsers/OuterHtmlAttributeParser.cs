using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// A parser for the x:outer-html attribute.
/// </summary>
public class OuterHtmlAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:outer-html or @data-htmt-outer-html]";

    public override void Parse(XmlNodeList? nodes)
    {
        // No nodes found
        if (nodes == null || nodes.Count == 0)
        {
            return;
        }

        foreach (var node in nodes)
        {
            if (node is not XmlElement n) continue;

            var outerHtmlVal = string.IsNullOrEmpty(n.GetAttribute("x:outer-html")) ? 
                n.GetAttribute("data-htmt-outer-html") : 
                n.GetAttribute("x:outer-html");
            
            n.RemoveAttribute("data-htmt-outer-html");
            n.RemoveAttribute("x:outer-html");

            if (string.IsNullOrEmpty(outerHtmlVal)) continue;

            var outerXml = new XmlDocument();
            outerXml.LoadXml($"<root>{ParseExpression(outerHtmlVal)}</root>");

            if (outerXml.DocumentElement?.FirstChild == null) continue;

            var importedNode = Xml.ImportNode(outerXml.DocumentElement.FirstChild, true);
            n.ParentNode?.ReplaceChild(importedNode, n);
        }
    }
}