using System.Xml;

namespace Htmt.AttributeParsers;

public class IfAttributeParser : IAttributeParser
{
    public string Name => "if";
    
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

            var key = Helper.GetAttributeValue(n, Name);
            var value = Helper.FindValueByKeys(data, key.Split('.'));
            
            if(value == null) continue;
            
            var removeNode = value switch
            {
                bool b => !b,
                int i => i == 0,
                double d => d == 0,
                string s => string.IsNullOrEmpty(s),
                _ => true
            };
            
            if (removeNode)
            {
                n.ParentNode?.RemoveChild(n);
            }
        }
    }
}