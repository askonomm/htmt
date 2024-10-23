using System;

namespace Htmt;

public interface IExpressionModifier
{
    public string Name { get; }

    public object? Modify(object? value, string? args = null);
}
