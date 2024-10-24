using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace Htmt.AttributeParsers;

public partial class BaseAttributeParser : IAttributeParser
{
    public virtual string XTag => throw new NotImplementedException();
    
    public XmlDocument Xml { get; set; } = new();
    
    public Dictionary<string, object?> Data { get; set; } = new();

    public IExpressionModifier[] ExpressionModifiers { get; set; } = [];
    
    [GeneratedRegex(@"(?<name>\{.*?\})")]
    private static partial Regex WholeKeyRegex();
    
    /**
     * This method is used to parse the expression.
     */
    protected string ParseExpression(string str)
    {
        var matches = WholeKeyRegex().Matches(str).Select(x => x.Groups["name"].Value).ToArray();

        foreach (var match in matches)
        {
            var strippedName = match[1..^1];
            var expressionModifierParser = new ExpressionModifierParser { Data = Data, ExpressionModifiers = ExpressionModifiers };
            var value = expressionModifierParser.Parse(strippedName);

            if (value != null)
            {
                str = value switch
                {
                    string s => str.Replace(match, s),
                    int i => str.Replace(match, i.ToString()),
                    double d => str.Replace(match, d.ToString(CultureInfo.CurrentCulture)),
                    bool b => str.Replace(match, b.ToString()),
                    _ => str.Replace(match, value.ToString()),
                };
            }
            else
            {
                str = str.Replace(match, "");
            }
        }

        return str;
    }
    
    /**
     * This method is used to parse the nodes.
     */
    public virtual void Parse(XmlNodeList? nodes)
    {
        throw new NotImplementedException();
    }
}