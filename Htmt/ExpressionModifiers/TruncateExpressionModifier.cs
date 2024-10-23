namespace Htmt.ExpressionModifiers;

public class TruncateExpressionModifier : IExpressionModifier
{
    public string Name => "truncate";

    public object? Modify(object? value, string? args = null)
    {
        if (value is not string s)
        {
            return value;
        }

        if (args is null || !int.TryParse(args, out var length))
        {
            return value;
        }

        return s.Length <= length ? value : s[..length];
    }
}