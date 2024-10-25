using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace Htmt.AttributeParsers;

/// <summary>
/// Base class for attribute parsers that provides some common functionality.
/// </summary>
public partial class BaseAttributeParser : IAttributeParser
{
    /// <summary>
    /// XML Tag selector for finding the relevant nodes.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public virtual string XTag => throw new NotImplementedException();
    
    /// <summary>
    /// The entire XML document that is being parsed.
    /// </summary>
    public XmlDocument Xml { get; set; } = new();
    
    /// <summary>
    /// Templating data.
    /// </summary>
    public Dictionary<string, object?> Data { get; set; } = new();

    /// <summary>
    /// List of expression modifiers.
    /// </summary>
    public IExpressionModifier[] ExpressionModifiers { get; set; } = [];
    
    [GeneratedRegex(@"(?<name>\{.*?\})")]
    private static partial Regex WholeKeyRegex();
    
    /// <summary>
    /// Parser the expression where it replaces variables with their data, and applies
    /// expression modifiers.
    /// </summary>
    /// <param name="str"></param>
    /// <returns>Returns the parsed expression as a string.</returns>
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
    
    /// <summary>
    /// A method that is called to parse the XML nodes.
    /// </summary>
    /// <param name="nodes"></param>
    /// <exception cref="NotImplementedException"></exception>
    public virtual void Parse(XmlNodeList? nodes)
    {
        throw new NotImplementedException();
    }
}