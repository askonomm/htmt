namespace Htmt;

public class ExpressionModifierParser
{
    public Dictionary<string, object?> Data { get; init; } = [];

    public IExpressionModifier[] ExpressionModifiers { get; init; } = [];

    public object? Parse(string expression)
    {
        var parts = expression.Split('|', StringSplitOptions.TrimEntries);
        var key = parts[0];
        var value = Helper.FindValueByKeys(Data, key.Split('.'));

        // No expression modifiers found
        if (parts.Length == 1)
        {
            return value;
        }

        var modifier = parts[1];
        var modifierName = modifier.Split(':', StringSplitOptions.TrimEntries)[0];
        var modifierArgs = modifier.Contains(':') ? modifier.Split(':', StringSplitOptions.TrimEntries)[1] : null;

        // Find the modifier instance
        var modifierInstance = ExpressionModifiers.FirstOrDefault(m => m.Name == modifierName);

        if (modifierInstance == null)
        {
            return value;
        }

        // Off to the races.
        return modifierInstance.Modify(value, modifierArgs);
    }
}
