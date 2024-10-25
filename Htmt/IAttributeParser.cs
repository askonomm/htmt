using System.Xml;

namespace Htmt;

/// <summary>
/// The interface for attribute parsers.
/// </summary>
public interface IAttributeParser
{
    /// <summary>
    /// The XML tag selector to find the elements to parse.
    /// </summary>
    public string XTag { get;  }
    
    /// <summary>
    /// The parent XML document.
    /// </summary>
    public XmlDocument Xml { get; set; }
    
    /// <summary>
    /// The templating data.
    /// </summary>
    public Dictionary<string, object?> Data { get; set; }
    
    /// <summary>
    /// The expression modifiers that the parser uses to modify expressions. Normally,
    /// this is passed to the ParseExpression method of BaseAttributeParser.
    /// </summary>
    public IExpressionModifier[] ExpressionModifiers { get; set; }
    
    /// <summary>
    /// The method that parses the XML nodes.
    /// </summary>
    /// <param name="nodes"></param>
    public void Parse(XmlNodeList? nodes);
}