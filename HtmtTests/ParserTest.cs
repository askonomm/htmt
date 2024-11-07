namespace HtmtTests;

[TestClass]
public class ParserTest
{
    [TestMethod]
    public void TestHtml5Document()
    {
        const string template =
            "<!DOCTYPE html><html><head><title x:inner-text=\"{title}\"></title></head><body><h1 x:inner-text=\"{heading}\"></h1></body></html>";
        var data = new Dictionary<string, object?>
            { { "title", "Hello, World!" }, { "heading", "Welcome to the world!" } };
        var parser = new Htmt.Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual(
            "<!DOCTYPE html><html><head><title>Hello, World!</title></head><body><h1>Welcome to the world!</h1></body></html>",
            html);
    }

    [TestMethod]
    public void TestHtml5DocumentWithComments()
    {
        const string template =
            "<!DOCTYPE html><!-- This is a comment --><html><head><title x:inner-text=\"{title}\"></title></head><body><h1 x:inner-text=\"{heading}\"></h1></body></html>";
        var data = new Dictionary<string, object?>
            { { "title", "Hello, World!" }, { "heading", "Welcome to the world!" } };
        var parser = new Htmt.Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual(
            "<!DOCTYPE html><html><head><title>Hello, World!</title></head><body><h1>Welcome to the world!</h1></body></html>",
            html);
    }

    [TestMethod]
    public void TestHtml4Document()
    {
        const string template =
            "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\"><html><head><title x:inner-text=\"{title}\"></title></head><body><h1 x:inner-text=\"{heading}\"></h1></body></html>";
        var data = new Dictionary<string, object?>
            { { "title", "Hello, World!" }, { "heading", "Welcome to the world!" } };
        var parser = new Htmt.Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual(
            "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\"><html><head><title>Hello, World!</title></head><body><h1>Welcome to the world!</h1></body></html>",
            html);
    }

    [TestMethod]
    public void TestXmlDocument()
    {
        const string template = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><title x:inner-text=\"{title}\"></title><heading x:inner-text=\"{heading}\"></heading>";
        var data = new Dictionary<string, object?>
            { { "title", "Hello, World!" }, { "heading", "Welcome to the world!" } };
        var parser = new Htmt.Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<?xml version=\"1.0\" encoding=\"UTF-8\"?><title>Hello, World!</title><heading>Welcome to the world!</heading>", html);
    }

    [TestMethod]
    public void TestVoidElButClosed()
    {
        const string template = "<html><head></head><body><img src=\"asd\" /></body></html>";
        var parser = new Htmt.Parser { Template = template };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><head></head><body><img src=\"asd\" /></body></html>", html);
    }

    [TestMethod]
    public void TestVoidElButOpen()
    {
        const string template = "<html><head></head><body><img src=\"asd\"><br><hr></body></html>";
        var parser = new Htmt.Parser { Template = template };

        Assert.AreEqual("<html><head></head><body><img src=\"asd\" /><br /><hr /></body></html>", parser.ToHtml());
    }

    [TestMethod]
    public void TestVoidElButOpen2()
    {
        const string template = "<html><head></head><body><ul><li>text<br></li></ul><hr></body></html>";
        var parser = new Htmt.Parser { Template = template };

        Assert.AreEqual("<html><head></head><body><ul><li>text<br /></li></ul><hr /></body></html>", parser.ToHtml());
    }

    [TestMethod]
    public void TestHtmlEntities()
    {
        const string template = "<html><head></head><body>&lt;div&gt;Hello, World!&lt;/div&gt;</body></html>";
        var parser = new Htmt.Parser { Template = template };

        Assert.AreEqual("<html><head></head><body><div>Hello, World!</div></body></html>", parser.ToHtml());
    }

    [TestMethod]
    public void TestMoreHtmlEntities()
    {
        const string template = "<html><head></head><body>&rarr;</body></html>";
        var parser = new Htmt.Parser { Template = template };

        Assert.AreEqual("<html><head></head><body>→</body></html>", parser.ToHtml());
    }

    [TestMethod]
    public void TestRemoveXmlnsFromChildren()
    {
        const string template = "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head></head><body><div x:for=\"items\" x:as=\"item\"><span x:inner-text=\"{item}\"></span></div></body></html>";
        var data = new Dictionary<string, object?> { { "items", new[] { "One", "Two", "Three" } } };
        var parser = new Htmt.Parser { Template = template, Data = data };

        Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head></head><body><div><span>One</span></div><div><span>Two</span></div><div><span>Three</span></div></body></html>", parser.ToHtml());
    }

    [TestMethod]
    public void TestFillVoidAttributes()
    {
        const string template = "<html><head></head><body><input type=\"checkbox\" checked /></body></html>";
        var parser = new Htmt.Parser { Template = template };
        
        Assert.AreEqual("<html><head></head><body><input type=\"checkbox\" checked=\"\" /></body></html>", parser.ToHtml());
    }
    
    [TestMethod]
    public void TestFillVoidAttributes2()
    {
        const string template = "<html><head><script defer src=\"\"></script></head><body></body></html>";
        var parser = new Htmt.Parser { Template = template };
        
        Assert.AreEqual("<html><head><script defer=\"\" src=\"\"></script></head><body></body></html>", parser.ToHtml());
    }

    [TestMethod]
    public void TestMultipleRootNodesInPartials()
    {
        const string template = "<html><head></head><body><div x:outer-partial=\"partial\"></div></body></html>";
        const string partial = "<div>hello</div><span x:inner-text=\"{item}\"></span>";
        var data = new Dictionary<string, object?> { { "partial", partial }, { "item", "world" } };
        var parser = new Htmt.Parser { Template = template, Data = data };
        
        Assert.AreEqual("<html><head></head><body><div>hello</div><span>world</span></body></html>", parser.ToHtml());
    }
}