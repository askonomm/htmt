using System.Globalization;
using System.Text.RegularExpressions;
using Htmt.ExpressionModifiers;

namespace Htmt;

public partial class Helper
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

    [GeneratedRegex(@"(?<name>\{.*?\})")]
    private static partial Regex WholeKeyRegex();

    public static string ReplaceKeysWithData(string str, Dictionary<string, object?> data)
    {
        var matches = WholeKeyRegex().Matches(str).Select(x => x.Groups["name"].Value).ToArray();

        foreach (var match in matches)
        {
            var strippedName = match[1..^1];

            var modifiers = new IExpressionModifier[]
            {
                new DateExpressionModifier()
            };

            var value = new ExpressionModifierParser { Data = data, ExpressionModifiers = modifiers }.Parse(strippedName);

            // print value
            Console.WriteLine(value);

            if (value != null)
            {
                str = value switch
                {
                    string s => str.Replace(match, s),
                    int i => str.Replace(match, i.ToString()),
                    double d => str.Replace(match, d.ToString(CultureInfo.CurrentCulture)),
                    bool b => str.Replace(match, b.ToString()),
                    _ => str.Replace(match, value.ToString()),
                };
            }
            else
            {
                str = str.Replace(match, "");
            }
        }

        return str;
    }
}