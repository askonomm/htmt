using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// A parser for the x:outer-partial attribute.
/// </summary>
public class OuterPartialAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:outer-partial]";

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

            var outerPartial = n.GetAttribute("x:outer-partial");
            n.RemoveAttribute("x:outer-partial");

            if (string.IsNullOrEmpty(outerPartial)) continue;

            if (Utils.FindValueByKeys(Data, outerPartial.Split('.')) is not string partial) continue;

            var newNode = new Parser { Data = Data, Template = partial }.ToXml();
            var parent = n.ParentNode;
            parent?.ReplaceChild(Xml.ImportNode(newNode, true), n);
        }
    }
}