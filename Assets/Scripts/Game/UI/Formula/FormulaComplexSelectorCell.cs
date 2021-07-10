/************************************************************************************
    作者：荆煦添
    描述：公式编辑器合成统计量方块绑定数据
*************************************************************************************/
using UnityEngine;

public class FormulaComplexSelectorCell : MonoBehaviour
{
    /// <summary>
    /// 直接测量量的统计不确定度
    /// </summary>
    public FormulaSelectorCell StatisticUncertainty;
    /// <summary>
    /// 直接测量量的平均值
    /// </summary>
    public FormulaSelectorCell StatisticAverage;
    /// <summary>
    /// 将数值和物理量名称加载到方块
    /// </summary>
    public void Show(string quantityName, FormulaController controller)
    {
        StatisticUncertainty.SetSelectorName(quantityName);
        StatisticAverage.SetSelectorName(quantityName);

        StatisticUncertainty.Title = "不确定度";
        StatisticUncertainty.Desc = $"物理量\"{quantityName}\"合成后的不确定度";

        StatisticAverage.Title = "平均值";
        StatisticAverage.Desc = $"物理量\"{quantityName}\"所有测量结果的平均值";

        StatisticUncertainty.FormulaControllerInstance = controller;
        StatisticAverage.FormulaControllerInstance = controller;
    }
}
