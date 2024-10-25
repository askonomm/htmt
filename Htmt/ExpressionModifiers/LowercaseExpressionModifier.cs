namespace Htmt.ExpressionModifiers;

/// <summary>
/// A modifier that converts a string to lowercase.
/// </summary>
public class LowercaseExpressionModifier : IExpressionModifier
{
    public string Name => "lowercase";

    public object? Modify(object? value, string? args = null)
    {
        return value is string s ? s.ToLower() : value;
    }
}