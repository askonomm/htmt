using Htmt;

namespace HtmtTests;

[TestClass]
public class AttributeParserTest
{
    [TestMethod]
    public void TestGenericValueAttributeParser()
    {
        const string template = "<html><body><a x:attr-href=\"{url}\" x:attr-title=\"Hello {name}\">Click here</a></body></html>";
        var data = new Dictionary<string, object?> { { "url", "https://www.example.com" }, { "name", "Example Website" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual(
            "<html><body><a href=\"https://www.example.com\" title=\"Hello Example Website\">Click here</a></body></html>",
            html);
    }

    [TestMethod]
    public void TestGenericValueAttributeParserAltSyntax()
    {
        const string template = "<html><body><a data-htmt-attr-href=\"{url}\" data-htmt-attr-title=\"Hello {name}\">Click here</a></body></html>";
        var data = new Dictionary<string, object?> { { "url", "https://www.example.com" }, { "name", "Example Website" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual(
            "<html><body><a href=\"https://www.example.com\" title=\"Hello Example Website\">Click here</a></body></html>",
            html);
    }

    [TestMethod]
    public void TestGenericValueAttributeParserWithoutData()
    {
        const string template = "<html><body><a x:attr-href=\"{url}\">Click here</a></body></html>";
        var parser = new Parser { Template = template };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><a href=\"\">Click here</a></body></html>", html);
    }

    [TestMethod]
    public void TestGenericValueAttributeParserWithoutDataAltSyntax()
    {
        const string template = "<html><body><a data-htmt-attr-href=\"{url}\">Click here</a></body></html>";
        var parser = new Parser { Template = template };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><a href=\"\">Click here</a></body></html>", html);
    }

    [TestMethod]
    public void TestInnerTextAttributeParser()
    {
        const string template = "<html><body><h1 x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }

    [TestMethod]
    public void TestInnerTextAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }

    [TestMethod]
    public void TestInnerHtmlAttributeParser()
    {
        const string template = "<html><body><div x:inner-html=\"{content}\"></div></body></html>";
        var data = new Dictionary<string, object?> { { "content", "<h1>Hello, World!</h1>" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><div><h1>Hello, World!</h1></div></body></html>", html);
    }
    
    [TestMethod]
    public void TestInnerHtmlAttributeParserAltSyntax()
    {
        const string template = "<html><body><div data-htmt-inner-html=\"{content}\"></div></body></html>";
        var data = new Dictionary<string, object?> { { "content", "<h1>Hello, World!</h1>" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div><h1>Hello, World!</h1></div></body></html>", html);
    }

    [TestMethod]
    public void TestOuterTextAttributeParser()
    {
        const string template = "<html><body><h1 x:outer-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body>Hello, World!</body></html>", html);
    }
    
    [TestMethod]
    public void TestOuterTextAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-outer-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body>Hello, World!</body></html>", html);
    }

    [TestMethod]
    public void TestOuterHtmlAttributeParser()
    {
        const string template = "<html><body><div x:outer-html=\"{content}\"></div></body></html>";
        var data = new Dictionary<string, object?> { { "content", "<h1>Hello, World!</h1>" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestOuterHtmlAttributeParserAltSyntax()
    {
        const string template = "<html><body><div data-htmt-outer-html=\"{content}\"></div></body></html>";
        var data = new Dictionary<string, object?> { { "content", "<h1>Hello, World!</h1>" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }

    [TestMethod]
    public void TestIfAttributeParser()
    {
        const string template = "<html><body><h1 x:if=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", true }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-if=\"show\" data-htmt-inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", true }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }

    [TestMethod]
    public void TestIfNotAttributeParser()
    {
        const string template = "<html><body><h1 x:if=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", false }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfNotAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-if=\"show\" data-htmt-inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", false }, { "title", "Hello, World!" } };
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
    public void TestIfNoDataAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-if=\"show\" data-htmt-inner-text=\"{title}\"></h1></body></html>";
        var parser = new Parser { Template = template };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }

    [TestMethod]
    public void TestIfListAttributeParser()
    {
        const string template = "<html><body><p x:if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string> { "Item 1", "Item 2", "Item 3" } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>There are items!</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfListAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string> { "Item 1", "Item 2", "Item 3" } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><p>There are items!</p></body></html>", html);
    }

    [TestMethod]
    public void TestIfListOfDictsAttributeParser()
    {
        const string template = "<html><body><p x:if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?>
        {
            {
                "items",
                new List<Dictionary<string, string>> { new() { { "key", "Item 1" } }, new() { { "key", "Item 2" } } }
            }
        };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>There are items!</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfListOfDictsAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?>
        {
            {
                "items",
                new List<Dictionary<string, string>> { new() { { "key", "Item 1" } }, new() { { "key", "Item 2" } } }
            }
        };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><p>There are items!</p></body></html>", html);
    }

    [TestMethod]
    public void TestIfEmptyListAttributeParser()
    {
        const string template = "<html><body><p x:if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string>() } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfEmptyListAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string>() } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }

    [TestMethod]
    public void TestIfDictionaryAttributeParser()
    {
        const string template = "<html><body><p x:if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new Dictionary<string, string> { { "key1", "Item 1" }, { "key2", "Item 2" } } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>There are items!</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfDictionaryAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new Dictionary<string, string> { { "key1", "Item 1" }, { "key2", "Item 2" } } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><p>There are items!</p></body></html>", html);
    }

    [TestMethod]
    public void TestIfEmptyDictionaryAttributeParser()
    {
        const string template = "<html><body><p x:if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new Dictionary<string, string>() } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestIfEmptyDictionaryAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-if=\"items\">There are items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new Dictionary<string, string>() } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }

    [TestMethod]
    public void TestComplexExpressionIfAttributeParser()
    {
        const string template = "<html><body><h1 x:if=\"(show is true) and (title is 'Hello, World!')\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", true }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestComplexExpressionIfAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-if=\"(show is true) and (title is 'Hello, World!')\" data-htmt-inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", true }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }

    [TestMethod]
    public void TestUnlessAttributeParser()
    {
        const string template = "<html><body><h1 x:unless=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", false }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-unless=\"show\" data-htmt-inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", false }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }

    [TestMethod]
    public void TestUnlessNotAttributeParser()
    {
        const string template = "<html><body><h1 x:unless=\"show\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", true }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessNotAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-unless=\"show\" data-htmt-inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", true }, { "title", "Hello, World!" } };
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
    public void TestUnlessNoDataAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-unless=\"show\" data-htmt-inner-text=\"{title}\"></h1></body></html>";
        var parser = new Parser { Template = template };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }

    [TestMethod]
    public void TestUnlessListAttributeParser()
    {
        const string template = "<html><body><p x:unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string> { "Item 1", "Item 2", "Item 3" } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessListAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string> { "Item 1", "Item 2", "Item 3" } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }

    [TestMethod]
    public void TestUnlessListOfDictsAttributeParser()
    {
        const string template = "<html><body><p x:unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?>
        {
            {
                "items",
                new List<Dictionary<string, string>> { new() { { "key", "Item 1" } }, new() { { "key", "Item 2" } } }
            }
        };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessListOfDictsAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?>
        {
            {
                "items",
                new List<Dictionary<string, string>> { new() { { "key", "Item 1" } }, new() { { "key", "Item 2" } } }
            }
        };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }

    [TestMethod]
    public void TestUnlessEmptyListAttributeParser()
    {
        const string template = "<html><body><p x:unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string>() } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>There are no items!</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessEmptyListAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string>() } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><p>There are no items!</p></body></html>", html);
    }

    [TestMethod]
    public void TestUnlessDictionaryAttributeParser()
    {
        const string template = "<html><body><p x:unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new Dictionary<string, string> { { "key1", "Item 1" }, { "key2", "Item 2" } } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessDictionaryAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new Dictionary<string, string> { { "key1", "Item 1" }, { "key2", "Item 2" } } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }

    [TestMethod]
    public void TestUnlessEmptyDictionaryAttributeParser()
    {
        const string template = "<html><body><p x:unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new Dictionary<string, string>() } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><p>There are no items!</p></body></html>", html);
    }
    
    [TestMethod]
    public void TestUnlessEmptyDictionaryAttributeParserAltSyntax()
    {
        const string template = "<html><body><p data-htmt-unless=\"items\">There are no items!</p></body></html>";
        var data = new Dictionary<string, object?> { { "items", new Dictionary<string, string>() } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><p>There are no items!</p></body></html>", html);
    }

    [TestMethod]
    public void TestComplexExpressionUnlessAttributeParser()
    {
        const string template = "<html><body><h1 x:unless=\"((show is true) and (title is 'Hello, World!')) is true\" x:inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", true }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body></body></html>", html);
    }
    
    [TestMethod]
    public void TestComplexExpressionUnlessAttributeParserAltSyntax()
    {
        const string template = "<html><body><h1 data-htmt-unless=\"((show is true) and (title is 'Hello, World!')) is true\" data-htmt-inner-text=\"{title}\"></h1></body></html>";
        var data = new Dictionary<string, object?> { { "show", true }, { "title", "Hello, World!" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body></body></html>", html);
    }

    [TestMethod]
    public void TestForAttributeParser()
    {
        const string template = "<html><body><ul><li x:for=\"items\" x:as=\"item\"><span x:outer-text=\"{item}\" /></li></ul></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string> { "Item 1", "Item 2", "Item 3" } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><ul><li>Item 1</li><li>Item 2</li><li>Item 3</li></ul></body></html>", html);
    }

    [TestMethod]
    public void TestForAttributeParserAltSyntax()
    {
        const string template = "<html><body><ul><li data-htmt-for=\"items\" data-htmt-as=\"item\"><span data-htmt-outer-text=\"{item}\" /></li></ul></body></html>";
        var data = new Dictionary<string, object?> { { "items", new List<string> { "Item 1", "Item 2", "Item 3" } } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><ul><li>Item 1</li><li>Item 2</li><li>Item 3</li></ul></body></html>", html);
    }

    [TestMethod]
    public void TestInnerPartialAttributeParser()
    {
        const string template = "<html><body><div x:inner-partial=\"partial\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var data = new Dictionary<string, object?> { { "partial", partial } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><div><h1>Hello, World!</h1></div></body></html>", html);
    }
    
    [TestMethod]
    public void TestInnerPartialAttributeParserAltSyntax()
    {
        const string template = "<html><body><div data-htmt-inner-partial=\"partial\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var data = new Dictionary<string, object?> { { "partial", partial } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div><h1>Hello, World!</h1></div></body></html>", html);
    }

    [TestMethod]
    public void TestInnerPartialAttributeParserWithData()
    {
        const string template = "<html><body><div x:inner-partial=\"partial\" /></body></html>";
        const string partial = "<h1 x:inner-text=\"Hello, {name}!\"></h1>";
        var data = new Dictionary<string, object?> { { "partial", partial }, { "name", "World" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><div><h1>Hello, World!</h1></div></body></html>", html);
    }
    
    [TestMethod]
    public void TestInnerPartialAttributeParserWithDataAltSyntax()
    {
        const string template = "<html><body><div data-htmt-inner-partial=\"partial\" /></body></html>";
        const string partial = "<h1 data-htmt-inner-text=\"Hello, {name}!\"></h1>";
        var data = new Dictionary<string, object?> { { "partial", partial }, { "name", "World" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div><h1>Hello, World!</h1></div></body></html>", html);
    }

    [TestMethod]
    public void TestInnerPartialAttributeParserWithHtmlEntities()
    {
        const string template = "<html><body><div x:inner-partial=\"partial\" /></body></html>";
        const string partial = "<h1>This way &rarr;</h1>";
        var data = new Dictionary<string, object?> { { "partial", partial } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><div><h1>This way →</h1></div></body></html>", html);
    }
    
    [TestMethod]
    public void TestInnerPartialAttributeParserWithHtmlEntitiesAltSyntax()
    {
        const string template = "<html><body><div data-htmt-inner-partial=\"partial\" /></body></html>";
        const string partial = "<h1>This way &rarr;</h1>";
        var data = new Dictionary<string, object?> { { "partial", partial } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div><h1>This way →</h1></div></body></html>", html);
    }

    [TestMethod]
    public void TestInnerPartialAttributeParserWithInterpolatedExpression()
    {
        const string template = "<html><body><div x:inner-partial=\"partials.{partial}\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var partials = new Dictionary<string, object?> { { "something", partial } };
        var data = new Dictionary<string, object?> { { "partials", partials }, { "partial", "something" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><div><h1>Hello, World!</h1></div></body></html>", html);
    }
    
    [TestMethod]
    public void TestInnerPartialAttributeParserWithInterpolatedExpressionAltSyntax()
    {
        const string template = "<html><body><div data-htmt-inner-partial=\"partials.{partial}\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var partials = new Dictionary<string, object?> { { "something", partial } };
        var data = new Dictionary<string, object?> { { "partials", partials }, { "partial", "something" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div><h1>Hello, World!</h1></div></body></html>", html);
    }

    [TestMethod]
    public void TestInnerPartialAttributeParserWithInterpolatedExpressionNoData()
    {
        const string template = "<html><body><div x:inner-partial=\"partials.{partial}\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var partials = new Dictionary<string, object?> { { "something", partial } };
        var data = new Dictionary<string, object?> { { "partials", partials } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div /></body></html>", html);
    }
    
    [TestMethod]
    public void TestInnerPartialAttributeParserWithInterpolatedExpressionNoDataAltSyntax()
    {
        const string template = "<html><body><div data-htmt-inner-partial=\"partials.{partial}\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var partials = new Dictionary<string, object?> { { "something", partial } };
        var data = new Dictionary<string, object?> { { "partials", partials } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div /></body></html>", html);
    }

    [TestMethod]
    public void TestOuterPartialAttributeParser()
    {
        const string template = "<html><body><div x:outer-partial=\"partial\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var data = new Dictionary<string, object?> { { "partial", partial } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestOuterPartialAttributeParserAltSyntax()
    {
        const string template = "<html><body><div data-htmt-outer-partial=\"partial\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var data = new Dictionary<string, object?> { { "partial", partial } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }

    [TestMethod]
    public void TestOuterPartialAttributeParserWithData()
    {
        const string template = "<html><body><div x:outer-partial=\"partial\" /></body></html>";
        const string partial = "<h1 x:inner-text=\"Hello, {name}!\"></h1>";
        var data = new Dictionary<string, object?> { { "partial", partial }, { "name", "World" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestOuterPartialAttributeParserWithDataAltSyntax()
    {
        const string template = "<html><body><div data-htmt-outer-partial=\"partial\" /></body></html>";
        const string partial = "<h1 data-htmt-inner-text=\"Hello, {name}!\"></h1>";
        var data = new Dictionary<string, object?> { { "partial", partial }, { "name", "World" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }

    [TestMethod]
    public void TestOuterPartialAttributeParserWithHtmlEntities()
    {
        const string template = "<html><body><div x:outer-partial=\"partial\" /></body></html>";
        const string partial = "<h1>This way &rarr;</h1>";
        var data = new Dictionary<string, object?> { { "partial", partial } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();

        Assert.AreEqual("<html><body><h1>This way →</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestOuterPartialAttributeParserWithHtmlEntitiesAltSyntax()
    {
        const string template = "<html><body><div data-htmt-outer-partial=\"partial\" /></body></html>";
        const string partial = "<h1>This way &rarr;</h1>";
        var data = new Dictionary<string, object?> { { "partial", partial } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>This way →</h1></body></html>", html);
    }

    [TestMethod]
    public void TestOuterPartialAttributeParserWithInterpolatedExpression()
    {
        const string template = "<html><body><div x:outer-partial=\"partials.{partial}\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var partials = new Dictionary<string, object?> { { "something", partial } };
        var data = new Dictionary<string, object?> { { "partials", partials }, { "partial", "something" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }
    
    [TestMethod]
    public void TestOuterPartialAttributeParserWithInterpolatedExpressionAltSyntax()
    {
        const string template = "<html><body><div data-htmt-outer-partial=\"partials.{partial}\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var partials = new Dictionary<string, object?> { { "something", partial } };
        var data = new Dictionary<string, object?> { { "partials", partials }, { "partial", "something" } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><h1>Hello, World!</h1></body></html>", html);
    }

    [TestMethod]
    public void TestOuterPartialAttributeParserWithInterpolatedExpressionNoData()
    {
        const string template = "<html><body><div x:outer-partial=\"partials.{partial}\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var partials = new Dictionary<string, object?> { { "something", partial } };
        var data = new Dictionary<string, object?> { { "partials", partials } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div /></body></html>", html);
    }
    
    [TestMethod]
    public void TestOuterPartialAttributeParserWithInterpolatedExpressionNoDataAltSyntax()
    {
        const string template = "<html><body><div data-htmt-outer-partial=\"partials.{partial}\" /></body></html>";
        const string partial = "<h1>Hello, World!</h1>";
        var partials = new Dictionary<string, object?> { { "something", partial } };
        var data = new Dictionary<string, object?> { { "partials", partials } };
        var parser = new Parser { Template = template, Data = data };
        var html = parser.ToHtml();
        
        Assert.AreEqual("<html><body><div /></body></html>", html);
    }
}