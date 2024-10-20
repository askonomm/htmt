using System.Xml;

namespace Htmt.AttributeParsers;

public class InnerPartialAttributeParser : IAttributeParser
{
    public string XTag => "//*[@x:inner-partial]";
    
    public void Parse(XmlDocument xml, Dictionary<string, object?> data, XmlNodeList? nodes)
    {
        // No nodes found
        if (nodes == null || nodes.Count == 0)
        {
            return;
        }
        
        foreach (var node in nodes)
        {
            if (node is not XmlElement n) continue;
            
            var innerPartial = n.GetAttribute("x:inner-partial");
            n.RemoveAttribute("x:inner-partial");

            if (string.IsNullOrEmpty(innerPartial)) continue;

            if (Helper.FindValueByKeys(data, innerPartial.Split('.')) is not string partial) continue;

            n.InnerXml = new Parser { Data = data, Template = partial }.ToXml().OuterXml;
        }
    }
}