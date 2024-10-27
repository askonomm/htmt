using System.Collections;
using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// A parser for the x:unless attribute.
/// </summary>
public class UnlessAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:unless]";

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

            var key = n.GetAttribute("x:unless");
            n.RemoveAttribute("x:unless");

            // if key is a single word, we just check for a truthy value
            if (!key.Contains(' '))
            {
                var value = Utils.FindValueByKeys(Data, key.Split('.'));

                var removeNode = value switch
                {
                    bool b => b,
                    int i => i != 0,
                    double d => d != 0,
                    string s => !string.IsNullOrEmpty(s),
                    IEnumerable<object> e => e.Any(),
                    IDictionary d => d.Count > 0,
                    _ => true
                };

                if (removeNode)
                {
                    n.ParentNode?.RemoveChild(n);
                }
            }

            // if key contains multiple words, evaluate the expression with ExpressionValidator
            else
            {
                var validator = new ExpressionBooleanValidator { Expression = key, Data = Data };
                var result = validator.Validates();

                if (result)
                {
                    n.ParentNode?.RemoveChild(n);
                }
            }
        }
    }
}