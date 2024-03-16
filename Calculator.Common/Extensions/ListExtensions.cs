namespace Calculator.Common.Extensions;

public static class ListExtensions
{
    public static T TakeLast<T>(this List<T> items)
    {
        var lastItem = items.Last();
        items.RemoveAt(items.Count - 1);
        return lastItem;
    }
}
