using System.Text.RegularExpressions;
using System.Xml;

namespace Htmt.AttributeParsers;

public class InnerHtmlAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:inner-html]";

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

            var innerHtmlVal = n.GetAttribute("x:inner-html");
            n.RemoveAttribute("x:inner-html");

            if (string.IsNullOrEmpty(innerHtmlVal)) continue;

            var innerXml = new XmlDocument();
            innerXml.LoadXml($"<root>{ParseExpression(innerHtmlVal)}</root>");
            n.InnerXml = innerXml.DocumentElement?.InnerXml ?? string.Empty;
        }
    }
}