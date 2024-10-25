namespace Htmt;

/// <summary>
/// The interface for expression modifiers.
/// </summary>
public interface IExpressionModifier
{
    /// <summary>
    /// The name of the modifier.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The method that modifies the value.
    /// </summary>
    /// <param name="value">The value to modify.</param>
    /// <param name="args">The arguments to passed to the modifier.</param>
    /// <returns>The modified value.</returns>
    public object? Modify(object? value, string? args = null);
}
