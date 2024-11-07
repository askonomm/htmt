using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// A parser for the x:outer-partial attribute.
/// </summary>
public class OuterPartialAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:outer-partial or @data-htmt-outer-partial]";

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

            var outerPartial = string.IsNullOrEmpty(n.GetAttribute("x:outer-partial")) ? 
                n.GetAttribute("data-htmt-outer-partial") : 
                n.GetAttribute("x:outer-partial");
            
            n.RemoveAttribute("data-htmt-outer-partial");
            n.RemoveAttribute("x:outer-partial");

            if (string.IsNullOrEmpty(outerPartial)) continue;
            
            var outerPartialValue = ParseExpression(outerPartial);

            if (Utils.FindValueByKeys(Data, outerPartialValue.Split('.')) is not string partial) continue;

            var newNode = new Parser { Data = Data, Template = partial }.ToXml();
            var parent = n.ParentNode;
            parent?.ReplaceChild(Xml.ImportNode(newNode, true), n);
        }
    }
}