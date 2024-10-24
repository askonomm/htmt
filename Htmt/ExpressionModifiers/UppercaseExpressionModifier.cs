namespace Htmt.ExpressionModifiers;

public class UppercaseExpressionModifier : IExpressionModifier
{
    public string Name => "uppercase";

    public object? Modify(object? value, string? args = null)
    {
        return value is string s ? s.ToUpper() : value;
    }
}
