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
    public FormulaSelectorCell StatisticDeltaComplex;
    public FormulaSelectorCell StatisticKComplex;
    /// <summary>
    /// 将数值和物理量名称加载到方块
    /// </summary>
    public void Show(string quantityName, FormulaController controller)
    {
        StatisticUncertainty.SetSelectorName(quantityName);
        StatisticAverage.SetSelectorName(quantityName);
        StatisticDeltaComplex.SetSelectorName(quantityName);
        StatisticKComplex.SetSelectorName(quantityName);

        var quantity = RecordManager.tempRecord.quantities.Find(x => x.Symbol.Equals(quantityName));

        if (quantity.processMethod == 1)
            SetActiveHelper(true, true, false, false);
        else if (quantity.processMethod == 2)
            SetActiveHelper(true, false, true, false);
        else if (quantity.processMethod == 3)
            SetActiveHelper(true, false, false, true);
        else if (quantity.processMethod == 4)
            SetActiveHelper(false, false, false, true);

        StatisticUncertainty.Title = "不确定度";
        StatisticUncertainty.Desc = $"物理量\"{quantityName}\"合成后的不确定度";

        StatisticAverage.Title = "平均值";
        StatisticAverage.Desc = $"物理量\"{quantityName}\"所有测量结果的平均值";

        StatisticDeltaComplex.Title = "变化量的平均值";
        StatisticDeltaComplex.Desc = $"物理量\"{quantityName}\"经过逐差后的平均值";

        StatisticKComplex.Title = "变化率（斜率）";
        StatisticKComplex.Desc = $"物理量\"{quantityName}\"一元线性回归法或图示法处理数据后获得的斜率值";

        StatisticUncertainty.FormulaControllerInstance = controller;
        StatisticAverage.FormulaControllerInstance = controller;
        StatisticDeltaComplex.FormulaControllerInstance = controller;
        StatisticKComplex.FormulaControllerInstance = controller;
    }

    private void SetActiveHelper(bool a, bool b, bool c, bool d)
    {
        StatisticUncertainty.gameObject.SetActive(a);
        StatisticAverage.gameObject.SetActive(b);
        StatisticDeltaComplex.gameObject.SetActive(c);
        StatisticKComplex.gameObject.SetActive(d);
    }
}
