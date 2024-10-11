using Htmt;

namespace HtmtTests;

[TestClass]
public class HelperTest
{
    [TestMethod]
    public void TestReplaceKeysWithData()
    {
        var data = new Dictionary<string, object> { { "name", "John Doe" } };
        var result = Helper.ReplaceKeysWithData("Hello, {name}!", data);
        
        Assert.AreEqual("Hello, John Doe!", result);
    }
    
    [TestMethod]
    public void TestReplaceKeysWithDataWithMultipleKeys()
    {
        var data = new Dictionary<string, object> { { "name", "John Doe" }, { "age", 30 } };
        var result = Helper.ReplaceKeysWithData("Hello, {name}! You are {age} years old.", data);
        
        Assert.AreEqual("Hello, John Doe! You are 30 years old.", result);
    }

    [TestMethod]
    public void TestReplaceKeysWithDataWithEmptyData()
    {
        var result = Helper.ReplaceKeysWithData("Hello, {name}!", new Dictionary<string, object>());

        Assert.AreEqual("Hello, {name}!", result);
    }
}