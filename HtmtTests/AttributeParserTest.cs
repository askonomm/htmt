using Htmt;

namespace HtmtTests;

[TestClass]
public class AttributeParserTest
{
    [TestMethod]
    public void TestGenericValueAttributeParser()
    {
        const string template = "<html><body><a x:href=\"{url}\" x:title=\"Hello {name}\">Click here</a></body></html>";
        var data = new Dictionary<string, object> { { "url", "https://www.example.com" }, { "name", "Example Website" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><a href=\"https://www.example.com\" title=\"Hello Example Website\">Click here</a></body></html>", html);
    }
    
    [TestMethod]
    public void TestGenericValueAttributeParserWithouData()
    {
        const string template = "<html><body><a x:href=\"{url}\">Click here</a></body></html>";
        var parser = new Parser { Template = template };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><a href=\"\">Click here</a></body></html>", html);
    }
    
    [TestMethod]
    public void TestInnerTextAttributeParser()
    {
        const string template = "<html><body><h1 x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object> { { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestInnerHtmlAttributeParser()
    {
        const string template = "<html><body><div x:inner-html=\"{content}\"></div></body></html>";
        var data = new Dictionary<string, object> { { "content", "<h1>Hello, World!</h1>" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div><h1>Hello, World!</h1></div></body></html>", html);
    }
    
    [TestMethod]
    public void TestOuterTextAttributeParser()
    {
        const string template = "<html><body><h1 x:outer-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object> { { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body>Hello, World!</body></html>", html);
    }
    
    [TestMethod]
    public void TestOuterHtmlAttributeParser()
    {
        const string template = "<html><body><div x:outer-html=\"{content}\"></div></body></html>";
        var data = new Dictionary<string, object> { { "content", "<h1>Hello, World!</h1>" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfAttributeParser()
    {
        const string template = "<html><body><h1 x:if=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object> { { "show", true }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfNotAttributeParser()
    {
        const string template = "<html><body><h1 x:if=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object> { { "show", false }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfNoDataAttributeParser()
    {
        const string template = "<html><body><h1 x:if=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var parser = new Parser { Template = template };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessAttributeParser()
    {
        const string template = "<html><body><h1 x:unless=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object> { { "show", false }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessNotAttributeParser()
    {
        const string template = "<html><body><h1 x:unless=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object> { { "show", true }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessNoDataAttributeParser()
    {
        const string template = "<html><body><h1 x:unless=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var parser = new Parser { Template = template };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestForAttributeParser()
    {
        const string template = "<html><body><ul><li x:for=\"items\" x:as=\"item\"><span x:outer-text=\"{item}\" /></li></ul></body></html>";
        var data = new Dictionary<string, object> { { "items", new List<string> { "Item 1", "Item 2", "Item 3" } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><ul><li>Item 1</li><li>Item 2</li><li>Item 3</li></ul></body></html>", html);
    }
}