using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// A parser for the x:inner-partial attribute.
/// </summary>
public class InnerPartialAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:inner-partial or @data-htmt-inner-partial]";

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

            var innerPartial = string.IsNullOrEmpty(n.GetAttribute("x:inner-partial")) ? 
                n.GetAttribute("data-htmt-inner-partial") : 
                n.GetAttribute("x:inner-partial");
            
            n.RemoveAttribute("data-htmt-inner-partial");
            n.RemoveAttribute("x:inner-partial");

            if (string.IsNullOrEmpty(innerPartial)) continue;
            
            var innerPartialValue = ParseExpression(innerPartial);

            if (Utils.FindValueByKeys(Data, innerPartialValue.Split('.')) is not string partial) continue;

            n.InnerXml = new Parser { Data = Data, Template = partial }.ToXml().OuterXml;
        }
    }
}