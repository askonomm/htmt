using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// A parser for the x:for attribute.
/// </summary>
public class ForAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:for or @data-htmt-for]";

    public override void Parse(XmlNodeList? nodes)
    {
        // No nodes found
        if (nodes == null || nodes.Count == 0)
        {
            return;
        }

        Parallel.ForEach(nodes.Cast<XmlNode>(), node =>
        {
            if (node is not XmlElement n) return;

            var collection = string.IsNullOrEmpty(n.GetAttribute("x:for")) ? 
                n.GetAttribute("data-htmt-for") : 
                n.GetAttribute("x:for");
            
            var asVar = string.IsNullOrEmpty(n.GetAttribute("x:as")) ? 
                n.GetAttribute("data-htmt-as") : 
                n.GetAttribute("x:as");

            n.RemoveAttribute("x:for");
            n.RemoveAttribute("data-htmt-for");
            n.RemoveAttribute("x:as");
            n.RemoveAttribute("data-htmt-as");

            var value = Utils.FindValueByKeys(Data, collection.Split('.'));
            if (value is not IEnumerable<object> enumerable) return;

            var fragment = Xml.CreateDocumentFragment();

            foreach (var item in enumerable)
            {
                var iterationData = new Dictionary<string, object?>(Data);

                if (!string.IsNullOrEmpty(asVar))
                {
                    iterationData[asVar] = item;
                }

                var iterationParser = new Parser { Template = n.OuterXml, Data = iterationData };
                var itemXml = iterationParser.ToXml();
                var importedNode = Xml.ImportNode(itemXml, true);

                fragment.AppendChild(importedNode);
            }

            n.ParentNode?.ReplaceChild(fragment, n);
        });
    }
}