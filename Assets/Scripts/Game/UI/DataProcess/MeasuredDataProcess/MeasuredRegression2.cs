using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuredRegression2 : MeasuredUncertainty
{
    public Image previewImage;

    public Text Title5;
    public Button CallButton5;
    public Text Value5;
    public Text Title6;
    public Button CallButton6;
    public Text Value6;

    protected override void Start()
    {
        CallButton1.onClick.AddListener(() => ShowFormulaEditor(CallButton1, Value1, quantity.BExpression == null ?
            quantity.BExpression = new List<FormulaNode>() : quantity.BExpression));
        CallButton2.onClick.AddListener(() => ShowFormulaEditor(CallButton2, Value2, quantity.AExpression == null ?
            quantity.AExpression = new List<FormulaNode>() : quantity.AExpression));
        CallButton3.onClick.AddListener(() => ShowFormulaEditor(CallButton3, Value3, quantity.RelationExpression == null ?
            quantity.RelationExpression = new List<FormulaNode>() : quantity.RelationExpression));
        CallButton4.onClick.AddListener(() => ShowFormulaEditor(CallButton4, Value4, quantity.UaExpression == null ?
            quantity.UaExpression = new List<FormulaNode>() : quantity.UaExpression));
        CallButton5.onClick.AddListener(() => ShowFormulaEditor(CallButton5, Value5, quantity.UbExpression == null ?
            quantity.UbExpression = new List<FormulaNode>() : quantity.UbExpression));
        CallButton6.onClick.AddListener(() => ShowFormulaEditor(CallButton6, Value6, quantity.ComplexExpression == null ?
            quantity.ComplexExpression = new List<FormulaNode>() : quantity.ComplexExpression));
        formulaController.onSelectCell += FormulaController_onSelectCell;
    }

    public override void Show(QuantityModel quantity)
    {
        this.quantity = quantity;
        ShowImage();
        formulaController.Initialize();
        formulaController.gameObject.SetActive(false);
        Title4.text = $"{(quantity.nextValue == 0 ? 'b' : 'a')}估计的A类不确定度\n\n=";
        Title5.text = $"{(quantity.nextValue == 0 ? 'b' : 'a')}估计的B类不确定度\n\n=";
        Title6.text = $"{(quantity.nextValue == 0 ? 'b' : 'a')}估计的合成不确定度\n\n=";
        if (quantity.BExpression != null && quantity.BExpression.Count != 0)
        {
            Value1.text = "=" + StaticMethods.NumberFormat(quantity.BExpression.GetExpressionExecuted());
            CallButton1.image.sprite = Sprites[2];
        }
        else
        {
            Value1.text = "=0";
            CallButton1.image.sprite = Sprites[0];
        }
        if (quantity.AExpression != null && quantity.AExpression.Count != 0)
        {
            Value2.text = "=" + StaticMethods.NumberFormat(quantity.AExpression.GetExpressionExecuted());
            CallButton2.image.sprite = Sprites[2];
        }
        else
        {
            Value2.text = "=0";
            CallButton2.image.sprite = Sprites[0];
        }
        if (quantity.RelationExpression != null && quantity.RelationExpression.Count != 0)
        {
            Value3.text = "=" + StaticMethods.NumberFormat(quantity.RelationExpression.GetExpressionExecuted());
            CallButton3.image.sprite = Sprites[2];
        }
        else
        {
            Value3.text = "=0";
            CallButton3.image.sprite = Sprites[0];
        }
        if (quantity.UaExpression != null && quantity.UaExpression.Count != 0)
        {
            Value4.text = "=" + StaticMethods.NumberFormat(quantity.UaExpression.GetExpressionExecuted());
            CallButton4.image.sprite = Sprites[2];
        }
        else
        {
            Value4.text = "=0";
            CallButton4.image.sprite = Sprites[0];
        }
        if (quantity.UbExpression != null && quantity.UbExpression.Count != 0)
        {
            Value5.text = "=" + StaticMethods.NumberFormat(quantity.UbExpression.GetExpressionExecuted());
            CallButton5.image.sprite = Sprites[2];
        }
        else
        {
            Value5.text = "=0";
            CallButton5.image.sprite = Sprites[0];
        }
        if (quantity.ComplexExpression != null && quantity.ComplexExpression.Count != 0)
        {
            Value6.text = "=" + StaticMethods.NumberFormat(quantity.ComplexExpression.GetExpressionExecuted());
            CallButton6.image.sprite = Sprites[2];
        }
        else
        {
            Value6.text = "=0";
            CallButton6.image.sprite = Sprites[0];
        }
    }

    public override bool CheckAll(bool silent = false)
    {
        if(base.CheckAll(silent))
        {
            if (quantity.BExpression == null || quantity.BExpression.Count == 0)
            {
                if (!silent)
                    ShowModel("b估计的值还未填写");
                return false;
            }
            if (quantity.AExpression == null || quantity.AExpression.Count == 0)
            {
                if (!silent)
                    ShowModel("a估计的值还未填写");
                return false;
            }
            if (quantity.RelationExpression == null || quantity.RelationExpression.Count == 0)
            {
                if (!silent)
                    ShowModel("相关系数还未填写");
                return false;
            }
            return true;
        }
        return false;
    }

    private void ShowImage()
    {
        LatexEquationRender.Render("y=a+bx", previewImage.FitWidth);
    }
}
