namespace Htmt;

public class Utils
{
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