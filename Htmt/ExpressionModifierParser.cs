namespace Htmt;

/// <summary>
/// The parser that parses expressions and applies modifiers to the values.
/// </summary>
public class ExpressionModifierParser
{
    /// <summary>
    /// The data that the parser uses to find values.
    /// </summary>
    public required Dictionary<string, object?> Data { get; init; } = new();
    
    /// <summary>
    /// The expression modifiers that the parser uses to modify values.
    /// </summary>
    public required IExpressionModifier[] ExpressionModifiers { get; init; }
    
    /// <summary>
    /// Parses the expression and applies modifiers to the value.
    /// </summary>
    /// <param name="expression">The expression to parse.</param>
    /// <returns>The parsed value.</returns>
    public object? Parse(string expression)
    {
        var parts = expression.Split('|', StringSplitOptions.TrimEntries);
        var key = parts[0];
        var value = Utils.FindValueByKeys(Data, key.Split('.'));

        if (value == null)
        {
            return null;
        }

        if (parts.Length == 1)
        {
            return value;
        }

        var modifiers = parts.Skip(1);

        foreach (var modifier in modifiers)
        {
            value = ApplyModifier(value, modifier);
        }

        return value;
    }

    /// <summary>
    /// Applies a modifier to the value.
    /// </summary>
    /// <param name="value">The value to modify.</param>
    /// <param name="modifier">The modifier to apply.</param>
    /// <returns>The modified value.</returns>
    private object? ApplyModifier(object? value, string modifier)
    {
        var modifierName = modifier.Split(':', StringSplitOptions.TrimEntries)[0];
        var modifierArgs = modifier.Contains(':') ? modifier.Split(':', StringSplitOptions.TrimEntries)[1] : null;
        var modifierInstance = ExpressionModifiers.FirstOrDefault(m => m.Name == modifierName);

        return modifierInstance == null ? value : modifierInstance.Modify(value, modifierArgs);
    }
}
