/************************************************************************************
    作者：张峻凡
    描述：直接测量量的处理流程
*************************************************************************************/
using HT.Framework;
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
        GameManager.Instance._currentQuantityIndex = 0;
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
        //根据记录的数据给公式编辑器传值
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
            result = StaticMethods.Average(CurrentQuantity.Data.ToDouble()).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.SigmaX)
        {
            result = (StaticMethods.CenterMoment(CurrentQuantity.Data.ToDouble(), 1) * CurrentQuantity.Groups).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.SigmaXSquare)
        {
            result = (StaticMethods.CenterMoment(CurrentQuantity.Data.ToDouble(), 2) * CurrentQuantity.Groups).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.SigmaXCube)
        {
            result = (StaticMethods.CenterMoment(CurrentQuantity.Data.ToDouble(), 3) * CurrentQuantity.Groups).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.Variance)
        {
            result = StaticMethods.Variance(CurrentQuantity.Data.ToDouble()).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.StandardDeviation)
        {
            result = StaticMethods.StdDev(CurrentQuantity.Data.ToDouble()).ToString();
        }
        else if (valueKind == MeasuredStatisticValue.InstrumentError)
        {
            result = GameManager.Instance.GetInstrument(CurrentQuantity.InstrumentType).ErrorLimit.ToString();
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
