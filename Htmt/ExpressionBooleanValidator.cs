﻿using System.Data;
using System.Text.RegularExpressions;

namespace Htmt;

public class ExpressionBooleanValidator
{
    public required string Expression { get; init; }
    
    public required Dictionary<string, object?> Data { get; init; }
    
    public bool Validates()
    {
        var normalizedExp = Normalize();
            
        return EvaluateExp(normalizedExp);
    }

    private string Normalize()
    {
        var evaluatedExpression = Expression;
        var keyMatches = Regex.Matches(Expression, """'(?<string>[^']*)'|(?<key>\w+)""");

        foreach (Match match in keyMatches)
        {
            // if this is not a key, skip
            if (match.Groups["key"].Success == false) continue;

            var key = match.Groups["key"].Value;

            // skip if the key is an operator
            if (key is "is" or "and" or "or" or "not") continue;

            // skip if key is integer, double, float, boolean, null or string
            if (int.TryParse(key, out _)) continue;
            if (double.TryParse(key, out _)) continue;
            if (float.TryParse(key, out _)) continue;
            if (bool.TryParse(key, out _)) continue;
            if (key == "null") continue;

            var value = Utils.FindValueByKeys(Data, key.Split('.'));
            var regex = new Regex($@"{key}");

            // if value is bool, lowercase it
            if (value is bool b)
            {
                evaluatedExpression = regex.Replace(evaluatedExpression, b.ToString().ToLower(), 1);
                continue;
            }

            evaluatedExpression = regex.Replace(evaluatedExpression, value?.ToString() ?? "null", 1);
        }

        // remove single quotes from string values
        return Regex.Replace(evaluatedExpression, "'(.*?)'", "$1");
    }

    private static bool EvaluateExp(string exp)
    {
        while (exp.Contains('('))
        {
            var openIndex = exp.LastIndexOf('(');
            var closeIndex = exp.IndexOf(')', openIndex);

            if (openIndex == -1 || closeIndex == -1)
            {
                throw new SyntaxErrorException("Invalid expression");
            }

            var subExp = exp.Substring(openIndex + 1, closeIndex - openIndex - 1);
            var subResult = EvaluateSimpleExp(subExp);

            exp = string.Concat(exp.AsSpan(0, openIndex), subResult.ToString().ToLower(), exp.AsSpan(closeIndex + 1));
        }

        return EvaluateSimpleExp(exp);
    }

    private static bool EvaluateSimpleExp(string exp)
    {
        var orParts = exp.Split([" or "], StringSplitOptions.None);

        foreach (var orPart in orParts)
        {
            var andParts = orPart.Split([" and "], StringSplitOptions.None);
            var andResult = andParts.Aggregate(true, (current, andPart) => current & EvaluateCondition(andPart.Trim()));

            if (andResult) return true;
        }

        return false;
    }

    private static bool EvaluateCondition(string condition)
    {
        if (condition == "true") return true;
        if (condition == "false") return false;

        if (condition.Contains(" is "))
        {
            var parts = condition.Split([" is "], StringSplitOptions.None);
            var key = parts[0].Trim();
            var value = parts[1].Trim();

            return key == value;
        }

        if (condition.Contains(" not "))
        {
            var parts = condition.Split([" not "], StringSplitOptions.None);
            var key = parts[0].Trim();
            var value = parts[1].Trim();

            return key != value;
        }

        throw new SyntaxErrorException("Invalid condition");
    }
}