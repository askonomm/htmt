using System.Xml;

namespace Htmt.AttributeParsers;

public class GenericValueAttributeParser : IAttributeParser
{
    public string XTag => "//*[@*[starts-with(name(), 'x:')]]";

    public void Parse(XmlDocument xml, Dictionary<string, object?> data, XmlNodeList? nodes)
    {
        // No nodes found
        if (nodes == null || nodes.Count == 0)
        {
            return;
        }

        foreach (var node in nodes)
        {
            if (node is not XmlElement n) continue;

            var attributes = n.Attributes.Cast<XmlAttribute>()
                .Where(a => a.Name.StartsWith("x:"))
                .ToList();

            foreach (var attr in attributes)
            {
                var val = n.GetAttribute(attr.Name);
                var newVal = Helper.ReplaceKeysWithData(val, data);
                n.SetAttribute(attr.Name[2..], newVal);
                n.RemoveAttribute(attr.Name);
            }
        }
    }
}