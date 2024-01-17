using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ArrayExtensions
{
    public static T PickRandom<T>(this T[] source)
    {
        int randomIndex = Random.Range(0, source.Length);
        if(randomIndex >= source.Length)
            return default(T);
        
        return source[randomIndex];
    }

    public static T PickRandom<T>(this List<T> source)
    {
        int randomIndex = Random.Range(0, source.Count);
        if(randomIndex >= source.Count)
            return default(T);
        
        return source[randomIndex];
    }
}