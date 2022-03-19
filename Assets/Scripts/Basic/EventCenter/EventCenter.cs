using System.Collections.Generic;
using UnityEngine;
using System;

public class EventCenter
{
    private static Dictionary<E_EventType, Delegate> myEvents = new Dictionary<E_EventType, Delegate>();

    #region 添加监听
    private static void _AddListener(E_EventType event_type, Delegate call_back)
    {
        if (!myEvents.ContainsKey(event_type))
        {
            myEvents.Add(event_type, null);
        }
        Delegate _d = myEvents[event_type];
        if (_d != null && _d.GetType() != call_back.GetType())
        {
            throw new Exception(string.Format("[{0}] Event type or paras wrong", event_type));
        }
    }
    public static void AddListener(E_EventType event_type, CallBack call_back)
    {
        _AddListener(event_type, call_back);
        myEvents[event_type] = (CallBack)myEvents[event_type] + call_back;
    }
    public static void AddListener<T>(E_EventType event_type, CallBack<T> call_back)
    {
        _AddListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T>)myEvents[event_type] + call_back;
    }
    public static void AddListener<T, X>(E_EventType event_type, CallBack<T, X> call_back)
    {
        _AddListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T, X>)myEvents[event_type] + call_back;
    }
    public static void AddListener<T, X, Y>(E_EventType event_type, CallBack<T, X, Y> call_back)
    {
        _AddListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T, X, Y>)myEvents[event_type] + call_back;
    }
    public static void AddListener<T, X, Y, Z>(E_EventType event_type, CallBack<T, X, Y, Z> call_back)
    {
        _AddListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T, X, Y, Z>)myEvents[event_type] + call_back;
    }
    public static void AddListener<T, X, Y, Z, W>(E_EventType event_type, CallBack<T, X, Y, Z, W> call_back)
    {
        _AddListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T, X, Y, Z, W>)myEvents[event_type] + call_back;
    }
    // remove listener
    #endregion

    #region 移除监听
    private static void _RemoveListener(E_EventType event_type, Delegate call_back)
    {
        if (myEvents.ContainsKey(event_type))
        {
            Delegate _d = myEvents[event_type];
            if (_d == null)
            {
                throw new Exception(string.Format("[{0}] 委托不存在!", event_type));
            }
            else if (_d.GetType() != call_back.GetType())
            {
                throw new Exception(string.Format("[{0}] 事件类型错误", event_type));
            }
        }
        else
        {
            throw new Exception(string.Format("[{0}] 事件不存在!", event_type));
        }
    }
    public static void RemoveListener(E_EventType event_type, CallBack call_back)
    {

        _RemoveListener(event_type, call_back);
        myEvents[event_type] = (CallBack)myEvents[event_type] - call_back;
    }
    public static void RemoveListener<T>(E_EventType event_type, CallBack<T> call_back)
    {
        _RemoveListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T>)myEvents[event_type] - call_back;
    }
    public static void RemoveListener<T, X>(E_EventType event_type, CallBack<T, X> call_back)
    {
        _RemoveListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T, X>)myEvents[event_type] - call_back;
    }
    public static void RemoveListener<T, X, Y>(E_EventType event_type, CallBack<T, X, Y> call_back)
    {
        _RemoveListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T, X, Y>)myEvents[event_type] - call_back;
    }
    public static void RemoveListener<T, X, Y, Z>(E_EventType event_type, CallBack<T, X, Y, Z> call_back)
    {
        _RemoveListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T, X, Y, Z>)myEvents[event_type] - call_back;
    }
    public static void RemoveListener<T, X, Y, Z, W>(E_EventType event_type, CallBack<T, X, Y, Z, W> call_back)
    {
        _RemoveListener(event_type, call_back);
        myEvents[event_type] = (CallBack<T, X, Y, Z, W>)myEvents[event_type] - call_back;
    }
    #endregion

    #region 广播
    // broadcast
    //0 para
    public static void Broadcast(E_EventType event_type)
    {
        Delegate _d;
        if (myEvents.TryGetValue(event_type, out _d))
        {
            CallBack call_back = _d as CallBack;
            if (call_back != null)
            {
                try
                {
                    call_back();
                }
                catch (MissingReferenceException)
                {
                    CleanDelegate(event_type, call_back);
                    throw new Exception("回调所拥有的物体不存在!");
                }
            }
            else
            {
                throw new Exception(string.Format("[{0}] 回调为空", event_type));
            }
        }
    }
    public static void Broadcast<T>(E_EventType event_type, T arg)
    {
        Delegate _d;
        if (myEvents.TryGetValue(event_type, out _d))
        {
            CallBack<T> call_back = _d as CallBack<T>;
            if (call_back != null)
            {
                try
                {
                    call_back(arg);
                }
                catch (MissingReferenceException)
                {
                    CleanDelegate(event_type, call_back);
                    throw new Exception("回调所拥有的物体不存在!");
                }
            }
            else
            {
                throw new Exception(string.Format("[{0}] 回调为空", event_type));
            }
        }
    }
    public static void Broadcast<T, X>(E_EventType event_type, T arg1, X arg2)
    {
        Delegate _d;
        if (myEvents.TryGetValue(event_type, out _d))
        {
            CallBack<T, X> call_back = _d as CallBack<T, X>;
            if (call_back != null)
            {
                try
                {
                    call_back(arg1, arg2);
                }
                catch (MissingReferenceException)
                {
                    CleanDelegate(event_type, call_back);
                    throw new Exception("回调所拥有的物体不存在!");
                }
            }
            else
            {
                throw new Exception(string.Format("[{0}] 回调为空", event_type));
            }
        }
    }
    public static void Broadcast<T, X, Y>(E_EventType event_type, T arg1, X arg2, Y arg3)
    {
        Delegate _d;
        if (myEvents.TryGetValue(event_type, out _d))
        {
            CallBack<T, X, Y> call_back = _d as CallBack<T, X, Y>;
            if (call_back != null)
            {
                try
                {
                    call_back(arg1, arg2, arg3);
                }
                catch (MissingReferenceException)
                {
                    CleanDelegate(event_type, call_back);
                    throw new Exception("回调所拥有的物体不存在!");
                }
            }
            else
            {
                throw new Exception(string.Format("[{0}] 回调为空", event_type));
            }
        }
    }
    public static void Broadcast<T, X, Y, Z>(E_EventType event_type, T arg1, X arg2, Y arg3, Z arg4)
    {
        Delegate _d;
        if (myEvents.TryGetValue(event_type, out _d))
        {
            CallBack<T, X, Y, Z> call_back = _d as CallBack<T, X, Y, Z>;
            if (call_back != null)
            {
                try
                {
                    call_back(arg1, arg2, arg3, arg4);
                }
                catch (MissingReferenceException)
                {
                    CleanDelegate(event_type, call_back);
                    throw new Exception("回调所拥有的物体不存在!");
                }
            }
            else
            {
                throw new Exception(string.Format("[{0}] 回调为空", event_type));
            }
        }
    }
    public static void Broadcast<T, X, Y, Z, W>(E_EventType event_type, T arg1, X arg2, Y arg3, Z arg4, W arg5)
    {
        Delegate _d;
        if (myEvents.TryGetValue(event_type, out _d))
        {
            CallBack<T, X, Y, Z, W> call_back = _d as CallBack<T, X, Y, Z, W>;
            if (call_back != null)
            {
                try
                {
                    call_back(arg1, arg2, arg3, arg4, arg5);
                }
                catch (MissingReferenceException)
                {
                    CleanDelegate(event_type, call_back);
                    throw new Exception("回调所拥有的物体不存在!");
                }
            }
            else
            {
                throw new Exception(string.Format("[{0}] 回调为空", event_type));
            }
        }
    }
    #endregion

    #region 清理监听

    private static string _CallbackWarning(Delegate _d)
    {
        return string.Format("脚本【{0}】的函数【{1}】回调错误，物体不存在！", _d.Target.GetType(), _d.Method);
    }
    public static void CleanDelegate(E_EventType event_type, CallBack call_back)
    {
        Delegate[] delegates = call_back.GetInvocationList();
        foreach (var item in delegates)
        {
            if (item.Target.ToString() != "null")
            {
            }
            else
            {
                Debug.LogWarning(_CallbackWarning(item));
                RemoveListener(event_type, (CallBack)item);
            }
        }
    }
    public static void CleanDelegate<T>(E_EventType event_type, CallBack<T> call_back)
    {
        Delegate[] delegates = call_back.GetInvocationList();
        foreach (var item in delegates)
        {
            if (item.Target.ToString() != "null")
            {
                //Debug.Log(item.Target + " 存在");
            }
            else
            {
                Debug.LogWarning(_CallbackWarning(item));
                RemoveListener(event_type, (CallBack<T>)item);
            }
        }
    }
    public static void CleanDelegate<T, X>(E_EventType event_type, CallBack<T, X> call_back)
    {
        Delegate[] delegates = call_back.GetInvocationList();
        foreach (var item in delegates)
        {
            if (item.Target.ToString() != "null")
            {
                //Debug.Log(item.Target + " 存在");
            }
            else
            {
                Debug.LogWarning(_CallbackWarning(item));
                RemoveListener(event_type, (CallBack<T, X>)item);
            }
        }
    }
    public static void CleanDelegate<T, X, Y>(E_EventType event_type, CallBack<T, X, Y> call_back)
    {
        Delegate[] delegates = call_back.GetInvocationList();
        foreach (var item in delegates)
        {
            if (item.Target.ToString() != "null")
            {
                //Debug.Log(item.Target + " 存在");
            }
            else
            {
                Debug.LogWarning(_CallbackWarning(item));
                RemoveListener(event_type, (CallBack<T, X, Y>)item);
            }
        }
    }
    public static void CleanDelegate<T, X, Y, Z>(E_EventType event_type, CallBack<T, X, Y, Z> call_back)
    {
        Delegate[] delegates = call_back.GetInvocationList();
        foreach (var item in delegates)
        {
            if (item.Target.ToString() != "null")
            {
                //Debug.Log(item.Target + " 存在");
            }
            else
            {
                Debug.LogWarning(_CallbackWarning(item));
                RemoveListener(event_type, (CallBack<T, X, Y, Z>)item);
            }
        }
    }
    public static void CleanDelegate<T, X, Y, Z, W>(E_EventType event_type, CallBack<T, X, Y, Z, W> call_back)
    {
        Delegate[] delegates = call_back.GetInvocationList();
        foreach (var item in delegates)
        {
            if (item.Target.ToString() != "null")
            {
                //Debug.Log(item.Target + " 存在");
            }
            else
            {
                Debug.LogWarning(_CallbackWarning(item));
                RemoveListener(event_type, (CallBack<T, X, Y, Z, W>)item);
            }
        }
    }
    #endregion
}
