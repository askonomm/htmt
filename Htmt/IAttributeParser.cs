using System.Xml;

namespace Htmt;

public interface IAttributeParser
{
    public void Parse(XmlDocument xml, Dictionary<string, object> data, XmlNodeList? nodes);
    
    public string Name { get; }
}