namespace Htmt.ExpressionModifiers;

/// <summary>
/// A modifier that capitalizes the first letter of a string.
/// </summary>
public class CapitalizeExpressionModifier : IExpressionModifier
{
    public string Name => "capitalize";

    public object? Modify(object? value, string? args = null)
    {
        if (value is not string s)
        {
            return value;
        }

        if (s.Length == 1)
        {
            return char.ToUpper(s[0]);
        }
        
        return char.ToUpper(s[0]) + s[1..];
    }
}