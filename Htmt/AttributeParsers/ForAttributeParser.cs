using System.Xml;

namespace Htmt.AttributeParsers;

public class ForAttributeParser : IAttributeParser
{
    public string Name => "for";
    
    private static string[] Attributes => ["for", "as"];
    
    public void Parse(XmlDocument xml, Dictionary<string, object> data, XmlNodeList? nodes)
    {
        // No nodes found
        if (nodes == null || nodes.Count == 0)
        {
            return; 
        }

        Parallel.ForEach(nodes.Cast<XmlNode>(), node =>
        {
            if (node is not XmlElement n) return;

            var collection = Helper.GetAttributeValue(n, Name);
            var asVar = Helper.GetAttributeValue(n, "as");

            // We need to remove the attributes from the node here already,
            // because we're passing the outerXml to the parser, 
            // and that would otherwise start infinitely parsing the For attributes.
            //
            // This is only a necessity with recursive parsing if you pass along the outerXml, 
            // otherwise you won't have to care because Htmt cleans up the attributes by itself by `Name`,
            // after the parser has been executed. Or in the case you use additional attributes,
            // you can remove them by yourself as well.
            Helper.RemoveAttributes(n, Attributes);

            var value = Helper.FindValueByKeys(data, collection.Split('.'));
            if (value is not IEnumerable<object> enumerable) return;

            var fragment = xml.CreateDocumentFragment();

            foreach (var item in enumerable)
            {
                var iterationData = new Dictionary<string, object>(data);

                if (!string.IsNullOrEmpty(asVar))
                {
                    iterationData[asVar] = item;
                }

                var iterationParser = new Parser { Template = n.OuterXml, Data = iterationData };
                var itemXml = iterationParser.ToXml();
                var importedNode = xml.ImportNode(itemXml, true);

                fragment.AppendChild(importedNode);
            }

            n.ParentNode?.ReplaceChild(fragment, n);
        });
    }
}