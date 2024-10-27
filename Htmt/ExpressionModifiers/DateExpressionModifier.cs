namespace Htmt.ExpressionModifiers;

/// <summary>
/// A modifier that formats a date.
/// </summary>
public class DateExpressionModifier : IExpressionModifier
{
    public string Name => "date";

    public object? Modify(object? value, string? args = null)
    {
        const string defaultFormat = "yyyy-MM-dd";
        
        if (value is string s)
        {
            return !DateTime.TryParse(s, out var dtParsed) ? value : dtParsed.ToString(args ?? defaultFormat);
        }

        return value is not DateTime dt ? value : dt.ToString(args ?? defaultFormat);
    }
}
