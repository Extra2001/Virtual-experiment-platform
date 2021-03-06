using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
/// <summary>
/// 新建流程
/// </summary>
public class EnterClassroomProcedure : ProcedureBase
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
        showed = false;
        Main.m_UI.OpenTemporaryUI<DatatableUILogic>();
        Main.m_UI.OpenResidentUI<GameButtonUILogic>();
        KeyboardManager.Instance.Register(KeyCode.T, () =>
        {
            if (showed)
                UIAPI.Instance.HideDataTable();
            else
                UIAPI.Instance.ShowDataTable();
            showed = !showed;
        });
        base.OnEnter(lastProcedure);
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        if (showed)
            UIAPI.Instance.HideDataTable();
        KeyboardManager.Instance.UnRegister(KeyCode.T);
        Main.m_UI.CloseUI<GameButtonUILogic>();
        base.OnLeave(nextProcedure);
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