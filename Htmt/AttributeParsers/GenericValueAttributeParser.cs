using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// A parser for generic value attributes.
/// </summary>
public class GenericValueAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@*[starts-with(name(), 'x:attr-')]]";

    public override void Parse(XmlNodeList? nodes)
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
                .Where(a => a.Name.StartsWith("x:attr-"))
                .ToList();

            foreach (var attr in attributes)
            {
                var val = n.GetAttribute(attr.Name);
                var newVal = ParseExpression(val);
                n.SetAttribute(attr.Name[7..], newVal);
                n.RemoveAttribute(attr.Name);
            }
        }
    }
}