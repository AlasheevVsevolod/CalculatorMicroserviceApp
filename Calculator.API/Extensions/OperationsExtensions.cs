using Calculator.API.Enums;

namespace Calculator.API.Extensions;

public static class OperationsExtensions
{
    public static bool TryGetOperation(this string value, out Operations operation)
    {
        operation = Operations.Add;
        var isPartOfEnum = IsOperation(value);
        if (!isPartOfEnum) return false;

        operation = Enum.GetValues<Operations>().First(o => GetValue(o) == value);
        return true;
    }

    private static bool IsOperation(string value)
    {
        var enumValues = Enum.GetValues<Operations>().Select(GetValue);

        return enumValues.Contains(value);
    }

    private static string GetValue(Operations operation)
    {
        return ((char)operation).ToString();
    }
}
