using System.Collections;
using System.Xml;

namespace Htmt.AttributeParsers;

public class UnlessAttributeParser : IAttributeParser
{
    public string XTag => "//*[@x:unless]";
    
    public void Parse(XmlDocument xml, Dictionary<string, object> data, XmlNodeList? nodes)
    {
        // No nodes found
        if (nodes == null || nodes.Count == 0)
        {
            return;
        }

        foreach (var node in nodes)
        {
            if (node is not XmlElement n) continue;

            var key = n.GetAttribute("x:unless");
            n.RemoveAttribute("x:unless");
            var value = Helper.FindValueByKeys(data, key.Split('.'));

            var removeNode = value switch
            {
                bool b => b,
                int i => i != 0,
                double d => d != 0,
                string s => !string.IsNullOrEmpty(s),
                IEnumerable<object> e => e.Any(),
                IDictionary d => d.Count > 0,
                _ => true
            };

            if (removeNode)
            {
                n.ParentNode?.RemoveChild(n);
            }
        }
    }
}