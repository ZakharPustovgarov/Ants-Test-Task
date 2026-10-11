
using System;

public class ClassUtility
{
    public static T CastOrThrow<T>(object unit, string context) where T : class
    {
        if (unit is T casted) return casted;
        throw new ArgumentException($"Class isn't {typeof(T).Name} in {context}");
    }
}