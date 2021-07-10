using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using System;
/// <summary>
/// 新建流程
/// </summary>
public class OnChair : ProcedureBase
{
    //选定位置，进入测量阶段

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
        RenderManager.Instance?.Show();

        GameManager.Instance.FPSable = true;//人物可否移动
        GameManager.Instance.PersonPosition = RecordManager.tempRecord.FPSPosition;//记录人物位置
        GameManager.Instance.PersonRotation = RecordManager.tempRecord.FPSRotation;

        base.OnEnter(lastProcedure);
        //打开相应UI
        Main.m_UI.OpenResidentUI<GameButtonUILogic>();

        Main.m_UI.OpenTemporaryUI<BagControl>();
        Main.m_UI.CloseUI<BagControl>();

        KeyboardManager.Instance.Register(KeyCode.T, () =>  //注册按键
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
        CreateObject.CreateRecord();   //复现存档仪器被测物体位置等信息
        CreateInstrument.CreateRecord();
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {

        //关闭UI，取消注册按键
        RenderManager.Instance?.Hide();
        GameManager.Instance.FPSable = false;
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
        RecordManager.tempRecord.FPSPosition = GameManager.Instance.PersonPosition.GetMyVector();//将当前人物位置存入存档
        RecordManager.tempRecord.FPSRotation = GameManager.Instance.PersonRotation.GetMyVector();
    }

    /// <summary>
    /// 流程帧刷新（秒）
    /// </summary>
    public override void OnUpdateSecond()
    {
        base.OnUpdateSecond();
    }
}