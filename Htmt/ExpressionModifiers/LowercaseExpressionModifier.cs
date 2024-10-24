namespace Htmt.ExpressionModifiers;

public class LowercaseExpressionModifier : IExpressionModifier
{
    public string Name => "lowercase";

    public object? Modify(object? value, string? args = null)
    {
        return value is string s ? s.ToLower() : value;
    }
}