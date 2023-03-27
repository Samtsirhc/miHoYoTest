using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{
    public static void Debug<T>(T var)
    {
        UnityEngine.Debug.Log(var);
    }
    public static void Log<T>(T var)
    {
        UnityEngine.Debug.Log(var);
    }
    public static void Warning<T>(T var)
    {
        UnityEngine.Debug.LogWarning(var);
    }
    public static void Error<T>(T var)
    {
        UnityEngine.Debug.LogError(var);
    }
    public static void Tip<T>(T var)
    {
        UnityEngine.Debug.Log(var);
    }
}
