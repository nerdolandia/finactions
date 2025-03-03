namespace FinActions.Domain.Shared.Extensions;

public static class EnumExtensions
{
    public static List<T> GetEnumValues<T>() where T : Enum
        => [.. (T[])Enum.GetValues(typeof(T))];
}
