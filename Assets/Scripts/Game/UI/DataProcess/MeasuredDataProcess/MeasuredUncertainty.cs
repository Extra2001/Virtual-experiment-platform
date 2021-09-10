using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuredUncertainty : HTBehaviour
{
    public QuantityModel quantity;

    public FormulaController formulaController;

    public Button CallButton1;
    public Text Value1;
    public Button CallButton2;
    public Text Value2;
    public Button CallButton3;
    public Text Value3;
    public Button CallButton4;
    public Text Value4;

    public Sprite[] Sprites = new Sprite[3];

    private List<FormulaNode> currentNodes = null;
    private Button currentButton = null;
    private Text currentValue = null;

    // Start is called before the first frame update
    void Start()
    {
        CallButton1.onClick.AddListener(() => ShowFormulaEditor(CallButton1, Value1, quantity.AverageExpression == null ?
            quantity.AverageExpression = new List<FormulaNode>() : quantity.AverageExpression));
        CallButton2.onClick.AddListener(() => ShowFormulaEditor(CallButton2, Value2, quantity.UaExpression == null ?
            quantity.UaExpression = new List<FormulaNode>() : quantity.UaExpression));
        CallButton3.onClick.AddListener(() => ShowFormulaEditor(CallButton3, Value3, quantity.UbExpression == null ?
            quantity.UbExpression = new List<FormulaNode>() : quantity.UbExpression));
        CallButton4.onClick.AddListener(() => ShowFormulaEditor(CallButton4, Value4, quantity.ComplexExpression == null ?
            quantity.ComplexExpression = new List<FormulaNode>() : quantity.ComplexExpression));
        formulaController.onSelectCell += FormulaController_onSelectCell;
    }

    public void Show(QuantityModel quantity)
    {
        //按钮颜色，存储的表达式等等
        this.quantity = quantity;
        formulaController.Initialize();
        formulaController.gameObject.SetActive(false);
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

    public bool CheckAll()
    {
        if (quantity.UaExpression == null || quantity.UaExpression.Count == 0)
        {
            ShowModel("A类不确定度还未填写");
            return false;
        }
        if (quantity.UbExpression == null || quantity.UbExpression.Count == 0)
        {
            ShowModel("B类不确定度还未填写");
            return false;
        }
        if (quantity.ComplexExpression == null || quantity.ComplexExpression.Count == 0)
        {
            ShowModel("合成不确定度还未填写");
            return false;
        }
        return true;
    }

    private void FormulaController_onSelectCell()
    {
        try
        {
            currentValue.text = "=" + StaticMethods.NumberFormat(formulaController.ExpressionExecuted);
            currentNodes.Clear();
            currentNodes.AddRange(formulaController.Serialize());
            currentButton.image.sprite = Sprites[2];
        }
        catch
        {
            currentButton.image.sprite = Sprites[1];
        }
    }

    private void ShowFormulaEditor(Button button, Text text, List<FormulaNode> nodes)
    {
        currentButton = button;
        currentValue = text;
        currentNodes = nodes;
        formulaController.gameObject.SetActive(true);
        if (nodes != null && nodes.Count != 0)
            formulaController.LoadFormula(nodes);
        else formulaController.Initialize();
    }

    private void ShowModel(string message)
    {
        UIAPI.Instance.ShowModel(new ModelDialogModel()
        {
            ShowCancel = false,
            Title = new BindableString("错误"),
            Message = new BindableString(message)
        });
    }
}
