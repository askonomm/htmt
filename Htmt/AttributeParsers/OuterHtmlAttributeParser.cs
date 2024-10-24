using System.Xml;

namespace Htmt.AttributeParsers;

public class OuterHtmlAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:outer-html]";

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

            var outerHtmlVal = n.GetAttribute("x:outer-html");
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