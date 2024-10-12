using System.Text.RegularExpressions;
using System.Xml;

namespace Htmt.AttributeParsers;

public class InnerTextAttributeParser : IAttributeParser
{
    public string XTag => "//*[@x:inner-text]";
    
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

            var innerVal = n.GetAttribute("x:inner-text");
            
            if (string.IsNullOrEmpty(innerVal)) continue;
            
            n.InnerText = Helper.ReplaceKeysWithData(innerVal, data);
        }
    }
}