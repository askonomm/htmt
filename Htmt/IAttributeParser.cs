using System.Xml;

namespace Htmt;

public interface IAttributeParser
{
    public string XTag { get;  }
    
    public XmlDocument Xml { get; set; }
    
    public Dictionary<string, object?> Data { get; set; }
    
    public IExpressionModifier[] ExpressionModifiers { get; set; }
    
    public void Parse(XmlNodeList? nodes);
}