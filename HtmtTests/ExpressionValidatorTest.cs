using Htmt;

namespace HtmtTests;

[TestClass]
public class ExpressionValidatorTest
{
    [TestMethod]
    public void TestSimpleOr()
    {
        var data = new Dictionary<string, object?> { { "test", 1 } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "1 is 1 or 2 is 3", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "1 is 2 or 2 is 4", Data = data }.Validates();

        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestIntComparisons()
    {
        var data = new Dictionary<string, object?> { { "test", 1 } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "test is 1", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "test is 2", Data = data }.Validates();

        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestDoubleComparisons()
    {
        var data = new Dictionary<string, object?> { { "test", 1.1 } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "test is 1.1", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "test is 1.2", Data = data }.Validates();
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestFloatComparisons()
    {
        var data = new Dictionary<string, object?> { { "test", 1.1f } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "test is 1.1", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "test is 1.2", Data = data }.Validates();

        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestStringComparisons()
    {
        var data = new Dictionary<string, object?> { { "test", "test" } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "test is 'test'", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "test is 'test2'", Data = data }.Validates();

        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestBooleanComparisons()
    {
        var data = new Dictionary<string, object?> { { "test", true } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "test is true", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "test is false", Data = data }.Validates();

        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestNullComparisons()
    {
        var data = new Dictionary<string, object?> { { "test", null } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "test is null", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "test is not null", Data = data }.Validates();

        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestAnd()
    {
        var data = new Dictionary<string, object?> { { "test", 1 }, { "test2", 2 } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "test is 1 and test2 is 2", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "test is 1 and test2 is 3", Data = data }.Validates();

        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestComplexAndExpression()
    {
        var data = new Dictionary<string, object?> { { "test", 1 }, { "test2", 2 }, { "test3", 3 } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "((test is 1) and (test2 is 2)) or (test3 is 3)", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "((test is 1) and (test2 is 3)) or (test3 is 4)", Data = data }.Validates();
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestComplexOrExpression()
    {
        var data = new Dictionary<string, object?> { { "test", 1 }, { "test2", 2 }, { "test3", 3 } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "((test is 1) or (test2 is 2)) and (test3 is 3)", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "((test is 2) or (test2 is 1))", Data = data }.Validates();
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestComplexExpression()
    {
        var data = new Dictionary<string, object?> { { "test", 1 }, { "test2", 2 }, { "test3", 3 } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "((test is 1) or (test2 is 2)) and (test3 is 3)", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "((test is 2) or (test2 is 1))", Data = data }.Validates();
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }

    [TestMethod]
    public void TestMultipleStringComparisons()
    {
        var data = new Dictionary<string, object?> { { "test", "test" }, { "test2", "test2" } };
        var returnsTrue = new ExpressionBooleanValidator { Expression = "(test is 'test') and (test2 is 'test2')", Data = data }.Validates();
        var returnsFalse = new ExpressionBooleanValidator { Expression = "(test is 'test') and (test2 is 'test')", Data = data }.Validates();
        
        Assert.IsTrue(returnsTrue);
        Assert.IsFalse(returnsFalse);
    }
}