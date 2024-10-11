using System.Text.RegularExpressions;
using System.Xml;

namespace Htmt.AttributeParsers;

public class HrefAttributeParser : IAttributeParser
{
    public string Name => "href";
    
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
            
            var val = Helper.GetAttributeValue(n, Name);
            
            n.SetAttribute("href", Helper.ReplaceKeysWithData(val, data));
        }
    }
}