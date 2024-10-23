namespace Htmt.ExpressionModifiers;

public class ReverseExpressionModifier : IExpressionModifier
{
    public string Name => "reverse";

    public object? Modify(object? value, string? args = null)
    {
        if (value is not string s)
        {
            return value;
        }

        var charArray = s.ToCharArray();
        Array.Reverse(charArray);
        
        return new string(charArray);
    }
}