/************************************************************************************
    作者：张峻凡
    描述：直接测量量的处理流程
*************************************************************************************/
using HT.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 直接测量量的处理流程
/// </summary>
public class MeasuredDataProcessProcedure : ProcedureBase
{
    private QuantityModel CurrentQuantity; //用于判别当前是哪一种直接测量量

    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RenderManager._SetTips("");
#endif
        GameManager.Instance._currentQuantityIndex = 0;
        GameManager.Instance.ShowUncertainty();
        base.OnEnter(lastProcedure);
    }

    public void ShowUncertainty(QuantityModel quantity)
    {
        CurrentQuantity = quantity;
        Main.m_UI.CloseUI<MeasuredDataProcess>();
        Main.m_UI.OpenUI<MeasuredDataProcess>(quantity);
    }

    public string GetStatisticValue(MeasuredStatisticValue valueKind)
    {
        //根据记录的数据给公式编辑器传值
        string result = "error";

        if (valueKind == MeasuredStatisticValue.Symbol)
            result = CurrentQuantity.Symbol;
        else if (valueKind == MeasuredStatisticValue.Number)
            result = CurrentQuantity.MesuredData.data.Count.ToString();
        else if (valueKind == MeasuredStatisticValue.Average)
            result = StaticMethods.Average(CurrentQuantity.MesuredData.data.ToDouble()).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmaX)
            result = (StaticMethods.CenterMoment(CurrentQuantity.MesuredData.data.ToDouble(), 1) 
                * CurrentQuantity.MesuredData.data.Count).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmaXSquare)
            result = (StaticMethods.CenterMoment(CurrentQuantity.MesuredData.data.ToDouble(), 2) 
                * CurrentQuantity.MesuredData.data.Count).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmaXCube)
            result = (StaticMethods.CenterMoment(CurrentQuantity.MesuredData.data.ToDouble(), 3) 
                * CurrentQuantity.MesuredData.data.Count).ToString();
        else if (valueKind == MeasuredStatisticValue.Variance)
            result = StaticMethods.Variance(CurrentQuantity.MesuredData.data.ToDouble()).ToString();
        else if (valueKind == MeasuredStatisticValue.StandardDeviation)
            result = StaticMethods.StdDev(CurrentQuantity.MesuredData.data.ToDouble()).ToString();
        else if (valueKind == MeasuredStatisticValue.InstrumentError)
            result = GameManager.Instance.GetInstrument(CurrentQuantity.InstrumentType).ErrorLimit.ToString();
        else if (valueKind == MeasuredStatisticValue.DeltaNumber)
            result = CurrentQuantity.DifferencedData.data.Count.ToString();
        else if (valueKind == MeasuredStatisticValue.DeltaXAverage)
            result = StaticMethods.CenterMoment(CurrentQuantity.DifferencedData.data.ToDouble(), 1).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmaDeltaX)
            result = (StaticMethods.CenterMoment(CurrentQuantity.DifferencedData.data.ToDouble(), 1) 
                * CurrentQuantity.DifferencedData.data.Count).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmaDeltaXSquare)
            result = (StaticMethods.CenterMoment(CurrentQuantity.DifferencedData.data.ToDouble(), 2) 
                * CurrentQuantity.DifferencedData.data.Count).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmDeltaXCube)
            result = (StaticMethods.CenterMoment(CurrentQuantity.DifferencedData.data.ToDouble(), 3) 
                * CurrentQuantity.DifferencedData.data.Count).ToString();
        else if (valueKind == MeasuredStatisticValue.VarianceDelta)
            result = StaticMethods.Variance(CurrentQuantity.DifferencedData.data.ToDouble()).ToString();
        else if (valueKind == MeasuredStatisticValue.StandardDeviationDelta)
            result = StaticMethods.StdDev(CurrentQuantity.DifferencedData.data.ToDouble()).ToString();
        else if (valueKind == MeasuredStatisticValue.IndependentNumber)
            result = CurrentQuantity.IndependentData.data.Count.ToString();
        else if (valueKind == MeasuredStatisticValue.IndependentXAverage)
            result = StaticMethods.Average(CurrentQuantity.IndependentData.data.ToDouble()).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmaIndependentX)
            result = (StaticMethods.CenterMoment(CurrentQuantity.IndependentData.data.ToDouble(), 1) 
                * CurrentQuantity.IndependentData.data.Count).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmaIndependentXSquare)
            result = (StaticMethods.CenterMoment(CurrentQuantity.IndependentData.data.ToDouble(), 2) 
                * CurrentQuantity.IndependentData.data.Count).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmIndependentXCube)
            result = (StaticMethods.CenterMoment(CurrentQuantity.IndependentData.data.ToDouble(), 3) 
                * CurrentQuantity.IndependentData.data.Count).ToString();
        else if (valueKind == MeasuredStatisticValue.SigmaXAndIndependentX)
        {
            List<double> list = new List<double>();
            for(int i = 0; i < CurrentQuantity.IndependentData.data.Count; i++)
                list.Add(Convert.ToDouble(CurrentQuantity.IndependentData.data[i]) * 
                    Convert.ToDouble(CurrentQuantity.MesuredData.data[i]));
            result = (StaticMethods.CenterMoment(list, 1) * list.Count).ToString();
        }

        return result;
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
    InstrumentError,   //仪器误差限
    DeltaNumber,
    DeltaXAverage,
    SigmaDeltaX,
    SigmaDeltaXSquare,
    SigmDeltaXCube,
    VarianceDelta,
    StandardDeviationDelta,
    IndependentNumber,
    IndependentXAverage,
    SigmaIndependentX,
    SigmaIndependentXSquare,
    SigmIndependentXCube,
    SigmaXAndIndependentX,
}
