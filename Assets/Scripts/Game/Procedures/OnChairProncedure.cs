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
        Main.m_UI.OpenResidentUI<GameButtonUILogic>();
        KeyboardManager.Instance.Register(KeyCode.T, () =>
        {
            if (Main.m_UI.GetOpenedUI<DatatableUILogic>() != null)
                UIAPI.Instance.HideDataTable();
            else
                UIAPI.Instance.ShowDataTable();
        });
        KeyboardManager.Instance.Register(KeyCode.B, () =>
        {
            if (Main.m_UI.GetOpenedUI<BagControl>() == null)
                Main.m_UI.OpenTemporaryUI<BagControl>();
            else
                Main.m_UI.GetUI<BagControl>().Hide();
        });

        if (RecordManager.tempRecord.ShowedObject != null)
        {
            CreateObject.CreateWithoutDestory();
        }

        RTEditor.EditorObjectSelection.Instance.SelectionChanged += InstrumentClicked;

    }

    private void InstrumentClicked(RTEditor.ObjectSelectionChangedEventArgs selectionChangedEventArgs)
    {
        selectionChangedEventArgs.SelectedObjects.ForEach(x => Log.Info(x.name));
        Log.Info(selectionChangedEventArgs.GizmoType.ToString());
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        base.OnLeave(nextProcedure);
        UIAPI.Instance.HideDataTable();
        Main.m_UI.CloseUI<BagControl>();
        Main.m_UI.CloseUI<GameButtonUILogic>();
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