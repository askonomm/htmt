using System.Text.RegularExpressions;
using System.Xml;

namespace Htmt.AttributeParsers;

public partial class OuterTextAttributeParser : IAttributeParser
{
    public string Name => "outer-text";
    
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

            var outerVal = Helper.GetAttributeValue(n, Name);
            
            if (string.IsNullOrEmpty(outerVal)) continue;
            
            outerVal = Helper.ReplaceKeysWithData(outerVal, data);
            n.ParentNode?.ReplaceChild(xml.CreateTextNode(outerVal), n);
        }
    }
}