namespace Htmt;

public class ExpressionModifierParser
{
    public required Dictionary<string, object?> Data { get; init; } = new();
    
    public required IExpressionModifier[] ExpressionModifiers { get; init; }
    
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

    private object? ApplyModifier(object? value, string modifier)
    {
        var modifierName = modifier.Split(':', StringSplitOptions.TrimEntries)[0];
        var modifierArgs = modifier.Contains(':') ? modifier.Split(':', StringSplitOptions.TrimEntries)[1] : null;
        var modifierInstance = ExpressionModifiers.FirstOrDefault(m => m.Name == modifierName);

        return modifierInstance == null ? value : modifierInstance.Modify(value, modifierArgs);
    }
}
