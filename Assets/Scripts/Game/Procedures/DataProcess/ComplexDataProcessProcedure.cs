/************************************************************************************
    作者：张峻凡
    描述：最终合成量的处理流程
*************************************************************************************/
using System.Collections.Generic;
using HT.Framework;
using System.Linq;
/// <summary>
/// 最终合成量的处理流程
/// </summary>
public class ComplexDataProcessProcedure : ProcedureBase
{
    private List<QuantityModel> quantities => RecordManager.tempRecord.quantities;

    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RenderManager._SetTips("合成量传递数据处理部分。");
#endif
        Main.m_UI.OpenUI<ComplexDataUILogic>();
        base.OnEnter(lastProcedure);
    }

    public List<string> GetQuantitiesName()
    {
        return RecordManager.tempRecord.quantities.Select(x => x.Symbol).ToList();
    }

    public string GetStatisticValue(string quantityName, ComplexStatisticValue valueKind)
    {
        // 根据记录的数据给公式编辑器传值
        string result = "error";
        var item = quantities.Where(x => x.Symbol.Equals(quantityName)).FirstOrDefault();

        if (ComplexStatisticValue.Average == valueKind)
        {
            if (item.processMethod == 1)
                result = StaticMethods.Average(item.MesuredData.data.ToDouble()).ToString();
            else if (item.processMethod == 2)
                result = (StaticMethods.Average(item.DifferencedData.data.ToDouble()) / item.DifferencedData.data.Count).ToString();
            else if (item.processMethod == 3 && item.nextValue == 0)
                result = item.BExpression.GetExpressionExecuted().ToString();
            //else if (item.processMethod == 3 && item.nextValue == 1)
            //    result = item.AExpression.GetExpressionExecuted().ToString();//一元线性回归此处已阉割
            else if (item.processMethod == 4)
                result = string.IsNullOrEmpty(item.change_rate) ? "0" : item.change_rate;
        }
        else if (ComplexStatisticValue.Uncertain == valueKind)
        {
            result = item.ComplexExpression.GetExpressionExecuted().ToString();
        }
        return StaticMethods.NumberFormat(double.Parse(result));//因直接测量量处保留了有效位数
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<ComplexDataUILogic>();
        base.OnLeave(nextProcedure);
    }
}

public enum ComplexStatisticValue
{
    Average,
    Uncertain
}
