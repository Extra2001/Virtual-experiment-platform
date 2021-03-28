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
    private bool Inputable = true;

    private void Start()
    {
        // 去流程的生命周期函数注册监听，这只是个替代方案
        /*Register(KeyCode.B, () =>
        {
            // 不必判断当前流程，在流程的生命周期结束的函数取消注册就OK
            Main.m_UI.OpenTemporaryUI<BagControl>();
            Debug.Log("B");
        });*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Inputable)
            foreach (var item in registered)
                if (Input.GetKeyDown(item.Key))
                {
                    InputGap();
                    item.Value.Invoke();
                }
    }

    /// <summary>
    /// 注册按键，按下指定的按键会触发第二个委托。
    /// </summary>
    /// <param name="key"></param>
    /// <param name="action"></param>
    public void Register(KeyCode key, Action action)
    {
        if (registered.ContainsKey(key))
            throw new Exception("该按键已注册");
        registered.Add(key, action);
    }

    /// <summary>
    /// 取消注册按键，取消后按下按键不会触发。
    /// </summary>
    /// <param name="key"></param>
    public void UnRegister(KeyCode key)
    {
        if (registered.ContainsKey(key))
            registered.Remove(key);
    }

    private void InputGap()
    {
        Inputable = false;
        //禁止200ms内连续按下两个按键
        Task.Delay(200).ContinueWith((_) =>
        {
            Inputable = true;
        });
    }
}
