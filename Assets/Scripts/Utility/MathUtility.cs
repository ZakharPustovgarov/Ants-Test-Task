using System.Collections.Generic;
using UnityEngine;

public class MathUtility
{
    public static T GetRandomElement<T>(IList<T> collection)
    {
        return collection[GetRandomInt(0, collection.Count)];
    }

    public static int GetRandomInt(int minInclusive, int maxExclusive)
    {
        return Random.Range(minInclusive, maxExclusive);
    }
}
