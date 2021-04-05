using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
/// <summary>
/// 新建流程
/// </summary>
public class OnChair : ProcedureBase
{
    bool showed = false;
    /// <summary>
    /// 流程初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
    }

    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        base.OnEnter(lastProcedure);
        KeyboardManager.Instance.Register(KeyCode.T, () =>
        {
            if (showed)
                UIAPI.Instance.HideDataTable();
            else
                UIAPI.Instance.ShowDataTable();
            showed = !showed;
        });
        KeyboardManager.Instance.Register(KeyCode.B, () =>
        {
            // 不必判断当前流程，在流程的生命周期结束的函数取消注册就OK
            Main.m_UI.OpenTemporaryUI<BagControl>();
        });
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        base.OnLeave(nextProcedure);
        KeyboardManager.Instance.UnRegister(KeyCode.T);
        KeyboardManager.Instance.UnRegister(KeyCode.B);
    }

    /// <summary>
    /// 流程帧刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    /// <summary>
    /// 流程帧刷新（秒）
    /// </summary>
    public override void OnUpdateSecond()
    {
        base.OnUpdateSecond();
    }
}