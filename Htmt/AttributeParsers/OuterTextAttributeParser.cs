using System.Text.RegularExpressions;
using System.Xml;

namespace Htmt.AttributeParsers;

public class OuterTextAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:outer-text]";

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

            var outerVal = n.GetAttribute("x:outer-text");
            n.RemoveAttribute("x:outer-text");

            if (string.IsNullOrEmpty(outerVal)) continue;

            outerVal = ParseExpression(outerVal);
            n.ParentNode?.ReplaceChild(Xml.CreateTextNode(outerVal), n);
        }
    }
}