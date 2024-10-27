namespace Htmt;

/// <summary>
/// General utility methods class.
/// </summary>
public class Utils
{
    /// <summary>
    /// Finds a value in a (potentially nested) dictionary by an array of keys.
    /// </summary>
    /// <param name="data">The dictionary to search.</param>
    /// <param name="keys">The keys to search for.</param>
    /// <returns>The value if found, otherwise null.</returns>
    public static object? FindValueByKeys(Dictionary<string, object?> data, string[] keys)
    {
        while (true)
        {
            if (!data.TryGetValue(keys.First(), out var v))
            {
                return null;
            }

            if (keys.Length == 1)
            {
                return v;
            }

            switch (v)
            {
                case Dictionary<string, object?> dict:
                {
                    var newKeys = keys.Skip(1).ToArray();

                    data = dict;
                    keys = newKeys;
                    continue;
                }
                case Dictionary<string, string?> dict:
                {
                    var newKeys = keys.Skip(1).ToArray();

                    data = dict.ToDictionary(x => x.Key, object? (x) => x.Value);
                    keys = newKeys;
                    continue;
                }
                case Dictionary<string, int?> dict:
                {
                    var newKeys = keys.Skip(1).ToArray();

                    data = dict.ToDictionary(x => x.Key, x => (object?)x.Value);
                    keys = newKeys;
                    continue;
                }
                case Dictionary<string, bool?> dict:
                {
                    var newKeys = keys.Skip(1).ToArray();

                    data = dict.ToDictionary(x => x.Key, x => (object?)x.Value);
                    keys = newKeys;
                    continue;
                }

                default:
                    return null;
            }
        }
    }
}