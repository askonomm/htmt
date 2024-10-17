using Htmt;

namespace HtmtTests;

[TestClass]
public class ExpressionValidatorTest
{
    [TestMethod]
    public void TestSimpleOr()
    {
        var returnsTrue = new ExpressionValidator("(1 is 1) or (2 is 3)").Validates(new Dictionary<string, object?>());
        var returnsFalse = new ExpressionValidator("(1 is 2) or (2 is 3)").Validates(new Dictionary<string, object?>());
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestIntComparisons()
    {
        var returnsTrue = new ExpressionValidator("test is 1").Validates(new Dictionary<string, object?>() { { "test", 1 } });
        var returnsFalse = new ExpressionValidator("test is 1").Validates(new Dictionary<string, object?>() { { "test", 2 } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
    
    [TestMethod]
    public void TestDoubleComparisons()
    {
        var returnsTrue = new ExpressionValidator("test is 1.1").Validates(new Dictionary<string, object?>() { { "test", 1.1 } });
        var returnsFalse = new ExpressionValidator("test is 1.1").Validates(new Dictionary<string, object?>() { { "test", 1.2 } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
    
    [TestMethod]
    public void TestFloatComparisons()
    {
        var returnsTrue = new ExpressionValidator("test is 1.1").Validates(new Dictionary<string, object?>() { { "test", 1.1f } });
        var returnsFalse = new ExpressionValidator("test is 1.1").Validates(new Dictionary<string, object?>() { { "test", 1.2f } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
    
    [TestMethod]
    public void TestStringComparisons()
    {
        var returnsTrue = new ExpressionValidator("test is 'test'").Validates(new Dictionary<string, object?>() { { "test", "test" } });
        var returnsFalse = new ExpressionValidator("test is 'test'").Validates(new Dictionary<string, object?>() { { "test", "test2" } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
    
    [TestMethod]
    public void TestBooleanComparisons()
    {
        var returnsTrue = new ExpressionValidator("test is true").Validates(new Dictionary<string, object?>() { { "test", true } });
        var returnsFalse = new ExpressionValidator("test is true").Validates(new Dictionary<string, object?>() { { "test", false } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
    
    [TestMethod]
    public void TestNullComparisons()
    {
        var returnsTrue = new ExpressionValidator("test is null").Validates(new Dictionary<string, object?>() { { "test", null } });
        var returnsFalse = new ExpressionValidator("test is null").Validates(new Dictionary<string, object?>() { { "test", "test" } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
    
    [TestMethod]
    public void TestAnd()
    {
        var returnsTrue = new ExpressionValidator("(test is 1) and (test2 is 2)").Validates(new Dictionary<string, object?>() { { "test", 1 }, { "test2", 2 } });
        var returnsFalse = new ExpressionValidator("(test is 1) and (test2 is 2)").Validates(new Dictionary<string, object?>() { { "test", 1 }, { "test2", 3 } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
    
    [TestMethod]
    public void TestComplexAndExpression()
    {
        var returnsTrue = new ExpressionValidator("((test is 1) and (test2 is 2)) or (test3 is 3)").Validates(new Dictionary<string, object?>() { { "test", 1 }, { "test2", 2 }, { "test3", 3 } });
        var returnsFalse = new ExpressionValidator("((test is 1) and (test2 is 2))").Validates(new Dictionary<string, object?>() { { "test", 1 }, { "test2", 3 } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
    
    [TestMethod]
    public void TestComplexOrExpression()
    {
        var returnsTrue = new ExpressionValidator("((test is 1) or (test2 is 2)) and (test3 is 3)").Validates(new Dictionary<string, object?>() { { "test", 1 }, { "test2", 3 }, { "test3", 3 } });
        var returnsFalse = new ExpressionValidator("((test is 1) or (test2 is 2))").Validates(new Dictionary<string, object?>() { { "test", 3 }, { "test2", 3 } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
    
    [TestMethod]
    public void TestComplexExpression()
    {
        var returnsTrue = new ExpressionValidator("((test is 1) or (test2 is 2)) and (test3 is 3)").Validates(new Dictionary<string, object?>() { { "test", 1 }, { "test2", 3 }, { "test3", 3 } });
        var returnsFalse = new ExpressionValidator("((test is 1) or (test2 is 2))").Validates(new Dictionary<string, object?>() { { "test", 3 }, { "test2", 3 } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestMultipleStringComparisons()
    {
        var returnsTrue = new ExpressionValidator("(test is 'test') and (test2 is 'test2')").Validates(new Dictionary<string, object?>() { { "test", "test" }, { "test2", "test2" } });
        var returnsFalse = new ExpressionValidator("(test is 'test') and (test2 is 'test2')").Validates(new Dictionary<string, object?>() { { "test", "test" }, { "test2", "test3" } });
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
}