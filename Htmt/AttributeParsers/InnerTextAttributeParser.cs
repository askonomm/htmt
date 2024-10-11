using System.Text.RegularExpressions;
using System.Xml;

namespace Htmt.AttributeParsers;

public class InnerTextAttributeParser : IAttributeParser
{
    public string Name => "inner-text";
    
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

            var innerVal = Helper.GetAttributeValue(n, Name);
            
            if (string.IsNullOrEmpty(innerVal)) continue;
            
            n.InnerText = Helper.ReplaceKeysWithData(innerVal, data);
        }
    }
}