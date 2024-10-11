using System.Xml;

namespace Htmt;

public interface IAttributeParser
{
    public string Name { get; }
    
    public void Parse(XmlDocument xml, Dictionary<string, object> data, XmlNodeList? nodes);
}