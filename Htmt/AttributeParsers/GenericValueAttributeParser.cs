using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// A parser for generic value attributes.
/// </summary>
public class GenericValueAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@*[starts-with(name(), 'x:attr-') or starts-with(name(), 'data-htmt-attr-')]]";

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
                .Where(a => a.Name.StartsWith("x:attr-") || a.Name.StartsWith("data-htmt-attr-"))
                .ToList();

            foreach (var attr in attributes)
            {
                var val = n.GetAttribute(attr.Name);
                var newVal = ParseExpression(val);

                n.SetAttribute(attr.Name.StartsWith("x:attr-") ? attr.Name[7..] : attr.Name[15..], newVal);
                n.RemoveAttribute(attr.Name);
            }
        }
    }
}