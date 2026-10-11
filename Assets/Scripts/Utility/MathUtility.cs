using System.Collections.Generic;
using UnityEngine;

public class MathUtility
{
    public static T GetRandomElement<T>(IList<T> collection)
    {
        return collection[GetRandomInt(0, collection.Count-1)];
    }

    public static int GetRandomInt(int minInclusive, int maxInclusive)
    {
        return Mathf.RoundToInt(Random.Range(minInclusive, maxInclusive));
    }
}
