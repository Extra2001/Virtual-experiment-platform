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
    private QuantityModel CurrentQuantity;


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
        CurrentQuantity = quantity;
        Main.m_UI.CloseUI<MeasuredDataProcess>();
        Main.m_UI.OpenResidentUI<MeasuredDataProcess>(quantity);
    }

    public string GetStatisticValue(MeasuredStatisticValue valueKind)
    {
        string result = "error";
        if (valueKind == MeasuredStatisticValue.Symbol)
        {
            result = CurrentQuantity.Symbol;
        }
        else if (valueKind == MeasuredStatisticValue.Number)
        {
            result = CurrentQuantity.Groups.ToString();
        }
        else if (valueKind == MeasuredStatisticValue.Average)
        {
            result = StaticMethods.Average(CurrentQuantity.Data).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.SigmaX)
        {
            result = (StaticMethods.CenterMoment(CurrentQuantity.Data, 1) * CurrentQuantity.Groups).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.SigmaXSquare)
        {
            result = (StaticMethods.CenterMoment(CurrentQuantity.Data, 2) * CurrentQuantity.Groups).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.SigmaXCube)
        {
            result = (StaticMethods.CenterMoment(CurrentQuantity.Data, 3) * CurrentQuantity.Groups).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.Variance)
        {
            result = StaticMethods.Variance(CurrentQuantity.Data).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.StandardDeviation)
        {
            result = StaticMethods.StdDev(CurrentQuantity.Data).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.InstrumentError)
        {
            result = Main.m_Entity.GetEntities(CurrentQuantity.InstrumentType)[0].Cast<InstrumentBase>().ErrorLimit.ToString();
        }
        return result;

        //throw new NotImplementedException();
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
    Symbol,     //  符号
    Number,     //测得数据组数
    Average,    //平均值
    SigmaX,     //X_i求和
    SigmaXSquare,//(X_i)^2求和
    SigmaXCube,  //(X_i)^3求和
    Variance,    //方差
    StandardDeviation,//标准差
    InstrumentError   //仪器误差限
}
