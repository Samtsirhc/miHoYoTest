using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMath
{
    public static float GetAvarage(float[] _array)
    {
        float _avg = 0f;
        foreach (var item in _array)
        {
            _avg += item;
        }
        _avg /= _array.Length;
        return _avg;
    }

    public static float GetAvarage(List<float> _list)
    {
        float[] _array = _list.ToArray();
        return GetAvarage(_array);
    }

    public static float GetVariance(float[] _array)
    {
        float _avg = GetAvarage(_array);
        float _v = 0f;
        foreach (var item in _array)
        {
            _v += (item - _avg) * (item - _avg);
        }
        _v /= _array.Length;
        return _v;
    }

    public static float GetVariance(List<float> _list)
    {
        float[] _array = _list.ToArray();
        return GetVariance(_array);
    }
}
