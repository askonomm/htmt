using System.Xml;

namespace Htmt.AttributeParsers;

public class OuterHtmlAttributeParser : IAttributeParser
{
    public string Name => "outer-html";
    
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

            var outerHtmlVal = Helper.GetAttributeValue(n, Name);
            
            if (string.IsNullOrEmpty(outerHtmlVal)) continue;
            
            var outerXml = new XmlDocument();
            outerXml.LoadXml($"<root>{Helper.ReplaceKeysWithData(outerHtmlVal, data)}</root>");
            
            if (outerXml.DocumentElement?.FirstChild == null) continue;
            
            var importedNode = xml.ImportNode(outerXml.DocumentElement.FirstChild, true);
            n.ParentNode?.ReplaceChild(importedNode, n);
        }
    }
}