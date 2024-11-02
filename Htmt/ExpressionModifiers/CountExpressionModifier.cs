using System.Collections;

namespace Htmt.ExpressionModifiers;

/// <summary>
/// Counts the number of items in a collection or the length of a string.
/// </summary>
public class CountExpressionModifier : IExpressionModifier
{
    public string Name => "count";
    
    public object? Modify(object? value, string? args = null)
    {
        if (value is IEnumerable enumerable)
        {
            return enumerable.Cast<object?>().Count();
        }
        
        if (value is string str)
        {
            return str.Length;
        }

        return value;
    }
}