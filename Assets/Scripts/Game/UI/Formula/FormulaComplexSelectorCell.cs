using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaComplexSelectorCell : MonoBehaviour
{
    public FormulaSelectorCell StatisticUncertainty;
    public FormulaSelectorCell StatisticAverage;

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
