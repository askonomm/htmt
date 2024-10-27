namespace HtmtTests;

using Htmt;

[TestClass]
public class ExpressionModifierTest
{
    [TestMethod]
    public void TestDateModifier()
    {
        const string template = "<html><body><p x:inner-text=\"{date | date:yyyy}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "date", "2022-01-01" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>2022</p></body></html>", html);
    }

    [TestMethod]
    public void TestDateModifierWithNoArgs()
    {
        const string template = "<html><body><p x:inner-text=\"{date | date}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "date", "2022-01-01" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>2022-01-01</p></body></html>", html);
    }

    [TestMethod]
    public void TestDateModifierWithDateTimeVal()
    {
        const string template = "<html><body><p x:inner-text=\"{date | date:yyyy}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "date", DateTime.Parse("2022-01-01") } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>2022</p></body></html>", html);
    }

    [TestMethod]
    public void TestUppercaseModifier()
    {
        const string template = "<html><body><p x:inner-text=\"{text | uppercase}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", "hello, world!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>HELLO, WORLD!</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestUppercaseModifierWithNull()
    {
        const string template = "<html><body><p x:inner-text=\"{text | uppercase}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", null } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p></p></body></html>", html);
    }
    
    [TestMethod]
    public void TestUppercaseModifierWithInt()
    {
        const string template = "<html><body><p x:inner-text=\"{text | uppercase}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", 123 } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>123</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestLowercaseModifier()
    {
        const string template = "<html><body><p x:inner-text=\"{text | lowercase}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", "HELLO, WORLD!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>hello, world!</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestLowercaseModifierWithNull()
    {
        const string template = "<html><body><p x:inner-text=\"{text | lowercase}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", null } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p></p></body></html>", html);
    }
    
    [TestMethod]
    public void TestLowercaseModifierWithInt()
    {
        const string template = "<html><body><p x:inner-text=\"{text | lowercase}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", 123 } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>123</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestCapitalizeModifier()
    {
        const string template = "<html><body><p x:inner-text=\"{text | capitalize}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", "hello, world!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>Hello, world!</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestCapitalizeModifierWithNull()
    {
        const string template = "<html><body><p x:inner-text=\"{text | capitalize}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", null } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p></p></body></html>", html);
    }
    
    [TestMethod]
    public void TestCapitalizeModifierSingleChar()
    {
        const string template = "<html><body><p x:inner-text=\"{text | capitalize}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", "a" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>A</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestTruncateModifier()
    {
        const string template = "<html><body><p x:inner-text=\"{text | truncate:5}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", "hello, world!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>hello</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestTruncateModifierWithoutArgs()
    {
        const string template = "<html><body><p x:inner-text=\"{text | truncate}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", "hello, world!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>hello, world!</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestReverseModifier()
    {
        const string template = "<html><body><p x:inner-text=\"{text | reverse}\"></p></body></html>";
        var data = new Dictionary<string, object?> { { "text", "hello, world!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>!dlrow ,olleh</p></body></html>", html);
    }
}