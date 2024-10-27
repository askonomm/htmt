using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// A parser for the x:inner-text attribute.
/// </summary>
public class InnerTextAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:inner-text]";

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

            var innerVal = n.GetAttribute("x:inner-text");
            n.RemoveAttribute("x:inner-text");

            if (string.IsNullOrEmpty(innerVal)) continue;

            n.InnerText = ParseExpression(innerVal);
        }
    }
}