using System.Text.RegularExpressions;
using System.Xml;

namespace Htmt.AttributeParsers;

public class InnerHtmlAttributeParser: IAttributeParser
{
    public string Name => "inner-html";
    
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

            var innerHtmlVal = Helper.GetAttributeValue(n, Name);
            
            if (string.IsNullOrEmpty(innerHtmlVal)) continue;
            
            var innerXml = new XmlDocument();
            innerXml.LoadXml($"<root>{Helper.ReplaceKeysWithData(innerHtmlVal, data)}</root>");
            n.InnerXml = innerXml.DocumentElement?.InnerXml ?? string.Empty;
        }
    }
}