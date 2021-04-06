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
    bool showedDataTable = false;
    bool showedBag = false;
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
        showedBag = showedDataTable = false;
        base.OnEnter(lastProcedure);
        KeyboardManager.Instance.Register(KeyCode.T, () =>
        {
            if (showedDataTable)
                UIAPI.Instance.HideDataTable();
            else
                UIAPI.Instance.ShowDataTable();
            showedDataTable = !showedDataTable;
        });
        KeyboardManager.Instance.Register(KeyCode.B, () =>
        {
            if (showedBag)
                Main.m_UI.CloseUI<BagControl>();
            else
                Main.m_UI.OpenTemporaryUI<BagControl>();
            showedBag = !showedBag;
        });
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        base.OnLeave(nextProcedure);
        if (showedDataTable)
            UIAPI.Instance.HideDataTable();
        if (showedBag)
            Main.m_UI.CloseUI<BagControl>();
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