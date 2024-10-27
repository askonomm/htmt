namespace Htmt.ExpressionModifiers;

/// <summary>
/// A modifier that converts a string to uppercase.
/// </summary>
public class UppercaseExpressionModifier : IExpressionModifier
{
    public string Name => "uppercase";

    public object? Modify(object? value, string? args = null)
    {
        return value is string s ? s.ToUpper() : value;
    }
}
