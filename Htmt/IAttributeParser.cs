using System.Xml;

namespace Htmt;

public interface IAttributeParser
{
    public string XTag { get; }
    
    public void Parse(XmlDocument xml, Dictionary<string, object> data, XmlNodeList? nodes);
}