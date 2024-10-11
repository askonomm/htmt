namespace HtmtTests;

[TestClass]
public class ParserTest
{
    [TestMethod]
    public void TestHtml5Document()
    {
        const string template = "<!DOCTYPE html><html><head><title x:inner-text=\"{title}\"></title></head><body><h1 x:inner-text=\"{heading}\"></h1></body></html>";
        var data = new Dictionary<string, object> { { "title", "Hello, World!" }, { "heading", "Welcome to the world!" } };
        var parser = new Htmt.Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<!DOCTYPE html><html><head><title>Hello, World!</title></head><body><h1>Welcome to the world!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestHtml5DocumentWithComments()
    {
        const string template = "<!DOCTYPE html><!-- This is a comment --><html><head><title x:inner-text=\"{title}\"></title></head><body><h1 x:inner-text=\"{heading}\"></h1></body></html>";
        var data = new Dictionary<string, object> { { "title", "Hello, World!" }, { "heading", "Welcome to the world!" } };
        var parser = new Htmt.Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<!DOCTYPE html><html><head><title>Hello, World!</title></head><body><h1>Welcome to the world!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestHtml4Document()
    {
        const string template = "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\"><html><head><title x:inner-text=\"{title}\"></title></head><body><h1 x:inner-text=\"{heading}\"></h1></body></html>";
        var data = new Dictionary<string, object> { { "title", "Hello, World!" }, { "heading", "Welcome to the world!" } };
        var parser = new Htmt.Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\"><html><head><title>Hello, World!</title></head><body><h1>Welcome to the world!</h1></body></html>", html);
    }
}