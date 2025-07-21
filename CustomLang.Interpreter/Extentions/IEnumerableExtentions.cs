namespace CustomLang.Interpreter.Extentions;

public static class IEnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any<T>();
    }

    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
    {
        return enumerable ?? Enumerable.Empty<T>();
    }

}