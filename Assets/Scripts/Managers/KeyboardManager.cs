using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class KeyboardManager : SingletonBehaviorManager<KeyboardManager>
{
    //此脚本控制非连续按下的所有键盘按键（移动及卡尺夹紧除外），下行标注所有按键作用
    //ESC：暂停，E：坐上凳子，B：打开关闭背包，~~~~~~~~~~~~~~
    private Dictionary<KeyCode, Action> registered = new Dictionary<KeyCode, Action>();
    private Dictionary<KeyCode, Action> registeredHoldOn = new Dictionary<KeyCode, Action>();
    private Dictionary<int, Tuple<Func<bool>, Action>> registedCustom = new Dictionary<int, Tuple<Func<bool>, Action>>();

    private void FixedUpdate()
    {
        foreach (var item in registered)
            if (Input.GetKeyDown(item.Key))
                item.Value.Invoke();
        foreach (var item in registeredHoldOn)
            if (Input.GetKey(item.Key))
                item.Value.Invoke();
        foreach (var item in registedCustom)
            if (item.Value.Item1.Invoke())
                item.Value.Item2.Invoke();
    }

    /// <summary>
    /// 注册按键，按下指定的按键会触发回调。
    /// </summary>
    public void Register(KeyCode key, Action action)
    {
        if (registered.ContainsKey(key))
        {
            Debug.LogError($"{key}建已被注册");
            throw new Exception("该按键已注册");
        }
        if (registeredHoldOn.ContainsKey(key))
        {
            Debug.LogError($"{key}建已被注册");
            throw new Exception("该按键已注册");
        }
        //Debug.Log($"已注册{key}键");
        registered.Add(key, action);
    }

    /// <summary>
    /// 注册按键，长按键会持续触发回调。
    /// </summary>
    public void RegisterHoldOn(KeyCode key, Action action)
    {
        if (registered.ContainsKey(key))
        {
            Debug.LogError($"{key}建已被注册");
            throw new Exception("该按键已注册");
        }
        if (registeredHoldOn.ContainsKey(key))
        {
            Debug.LogError($"{key}建已被注册");
            throw new Exception("该按键已注册");
        }
        //Debug.Log($"已注册{key}键");
        registeredHoldOn.Add(key, action);
    }

    /// <summary>
    /// 取消注册按键，取消后按下按键不会触发。
    /// </summary>
    public void UnRegister(KeyCode key)
    {
        if (registered.ContainsKey(key))
            registered.Remove(key);
        else
            Debug.LogWarning($"未注册{key}的单击按键，无法取消注册");
    }

    public void UnRegisterHoldOn(KeyCode key)
    {
        if (registeredHoldOn.ContainsKey(key))
            registeredHoldOn.Remove(key);
        else
            Debug.LogWarning($"未注册{key}的长按按键，无法取消注册");
    }

    /// <summary>
    /// 注册自定义监听逻辑。
    /// </summary>
    /// <param name="KeyCondition"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public int Register(Func<bool> KeyCondition, Action action)
    {
        //Debug.Log("已注册自定义监听逻辑。");
        int id = 0;
        for (; registedCustom.ContainsKey(id); id++) ;
        registedCustom.Add(id, new Tuple<Func<bool>, Action>(KeyCondition, action));
        return id;
    }

    /// <summary>
    /// 取消自定义监听逻辑
    /// </summary>
    /// <param name="id"></param>
    public void UnRegister(int id)
    {
        if (registedCustom.ContainsKey(id))
            registedCustom.Remove(id);
        else
            Debug.LogWarning($"未注册id为{id}的监听逻辑，无法取消注册");
    }
}
