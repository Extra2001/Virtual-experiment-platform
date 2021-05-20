using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using System;
/// <summary>
/// 新建流程
/// </summary>
public class MeasuredDataProcessProcedure : ProcedureBase
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
        GameManager.Instance.ShowUncertainty();
        base.OnEnter(lastProcedure);
    }

    public void ShowUncertainty(QuantityModel quantity)
    {
        Main.m_UI.CloseUI<MeasuredDataProcess>();
        Main.m_UI.OpenResidentUI<MeasuredDataProcess>(quantity);
    }

    public string GetStatisticValue(MeasuredStatisticValue valueKind)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<MeasuredDataProcess>();
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

public enum MeasuredStatisticValue
{
    Number,     //测得数据组数
    Average,    //平均值
    SigmaX,     //X_i求和
    SigmaXSquare,//(X_i)^2求和
    SigmaXCube,  //(X_i)^3求和
    Variance,    //方差
    StandardDeviation,//标准差
    InstrumentError   //仪器误差限
}
