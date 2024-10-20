using System.Xml;

namespace Htmt.AttributeParsers;

public class OuterPartialAttributeParser : IAttributeParser
{
    public string XTag => "//*[@x:outer-partial]";
    
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
            
            var outerPartial = n.GetAttribute("x:outer-partial");
            n.RemoveAttribute("x:outer-partial");

            if (string.IsNullOrEmpty(outerPartial)) continue;

            if (Helper.FindValueByKeys(data, outerPartial.Split('.')) is not string partial) continue;

            var newNode = new Parser { Data = data, Template = partial }.ToXml();
            var parent = n.ParentNode;
            parent?.ReplaceChild(xml.ImportNode(newNode, true), n);
        }
    }
}