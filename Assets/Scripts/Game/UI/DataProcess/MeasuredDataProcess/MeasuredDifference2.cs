using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuredDifference2 : MeasuredUncertainty
{
    public override void Show(QuantityModel quantity)
    {
        //按钮颜色，存储的表达式等等
        this.quantity = quantity;
        formulaController.Initialize();
        formulaController.gameObject.SetActive(false);
        Title1.text = $"{"Δ" + quantity.Symbol}的平均值\n\n=";
        Title2.text = $"{"Δ" + quantity.Symbol}的A类不确定度\n\n=";
        Title3.text = $"{"Δ" + quantity.Symbol}的B类不确定度\n\n=";
        Title4.text = $"{"Δ" + quantity.Symbol}的合成不确定度\n\n=";
        if (quantity.AverageExpression != null && quantity.AverageExpression.Count != 0)
        {
            Value1.text = "=" + StaticMethods.NumberFormat(quantity.AverageExpression.GetExpressionExecuted());
            CallButton1.image.sprite = Sprites[2];
        }
        else
        {
            Value1.text = "=0";
            CallButton1.image.sprite = Sprites[0];
        }
        if (quantity.UaExpression != null && quantity.UaExpression.Count != 0)
        {
            Value2.text = "=" + StaticMethods.NumberFormat(quantity.UaExpression.GetExpressionExecuted());
            CallButton2.image.sprite = Sprites[2];
        }
        else
        {
            Value2.text = "=0";
            CallButton2.image.sprite = Sprites[0];
        }
        if (quantity.UbExpression != null && quantity.UbExpression.Count != 0)
        {
            Value3.text = "=" + StaticMethods.NumberFormat(quantity.UbExpression.GetExpressionExecuted());
            CallButton3.image.sprite = Sprites[2];
        }
        else
        {
            Value3.text = "=0";
            CallButton3.image.sprite = Sprites[0];
        }
        if (quantity.ComplexExpression != null && quantity.ComplexExpression.Count != 0)
        {
            Value4.text = "=" + StaticMethods.NumberFormat(quantity.ComplexExpression.GetExpressionExecuted());
            CallButton4.image.sprite = Sprites[2];
        }
        else
        {
            Value4.text = "=0";
            CallButton4.image.sprite = Sprites[0];
        }
    }

    public override bool CheckAll(bool silent = false)
    {
        if (quantity.AverageExpression == null || quantity.AverageExpression.Count == 0)
        {
            if (!silent)
                ShowModel("平均值还未填写");
            return false;
        }
        if (base.CheckAll(silent))
        {            
            return true;
        }
        return false;
    }
}
