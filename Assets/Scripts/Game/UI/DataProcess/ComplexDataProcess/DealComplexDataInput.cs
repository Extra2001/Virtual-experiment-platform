/************************************************************************************
    作者：张峻凡
    描述：处理合成量的数据输入
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DealComplexDataInput : HTBehaviour
{
    public Image FormulaImage;

    public Button CallButton1;
    public Text Value1;

    public Button CallButton2;
    public Text Value2;

    public InputField MainValue;
    public InputField Uncertain;

    public GameObject HidePanel;

    public Sprite[] Sprites = new Sprite[3];
    public FormulaController formulaController;

    protected List<FormulaNode> currentNodes = null;
    protected Button currentButton = null;
    protected Text currentValue = null;

    void Start()
    {
        ComplexQuantityModel model = RecordManager.tempRecord.complexQuantityModel;
        CallButton1.onClick.AddListener(() => ShowFormulaEditor(CallButton1, Value1, model.AverageExpression == null ?
             model.AverageExpression = new List<FormulaNode>() : model.AverageExpression));
        CallButton2.onClick.AddListener(() => ShowFormulaEditor(CallButton2, Value2, model.UncertainExpression == null ?
            model.UncertainExpression = new List<FormulaNode>() : model.UncertainExpression));
        MainValue.onValueChanged.AddListener(x => RecordManager.tempRecord.complexQuantityModel.AnswerAverage = x);
        Uncertain.onValueChanged.AddListener(x => RecordManager.tempRecord.complexQuantityModel.AnswerUncertain = x);
        formulaController.onSelectCell += FormulaController_onSelectCell;
    }
    
    public void Show()
    {
        RenderFormula();
        formulaController.Initialize();
        formulaController.gameObject.SetActive(false);
        ComplexQuantityModel model = RecordManager.tempRecord.complexQuantityModel;

        if (RecordManager.tempRecord.quantities.Where(x => x.processMethod == 4).Any())
            HidePanel.SetActive(false);
        else HidePanel.SetActive(true);

        if (model.AverageExpression != null && model.AverageExpression.Count != 0)
        {
            Value1.text = "=" + StaticMethods.NumberFormat(model.AverageExpression.GetExpressionExecuted());
            CallButton1.image.sprite = Sprites[2];
        }
        else
        {
            Value1.text = "=0";
            CallButton1.image.sprite = Sprites[0];
        }
        if (model.UncertainExpression != null && model.UncertainExpression.Count != 0)
        {
            Value2.text = "=" + StaticMethods.NumberFormat(model.UncertainExpression.GetExpressionExecuted());
            CallButton2.image.sprite = Sprites[2];
        }
        else
        {
            Value2.text = "=0";
            CallButton2.image.sprite = Sprites[0];
        }
        MainValue.text = model.AnswerAverage;
        Uncertain.text = model.AnswerUncertain;
    }

    private void RenderFormula()
    {
        var tex = EnterExpression.ExpressionToShow(RecordManager.tempRecord.stringExpression);
        LatexEquationRender.Render(tex, res =>
        {
            FormulaImage.FitImage(res, 270, 40);
        });
    }

    protected void ShowFormulaEditor(Button button, Text text, List<FormulaNode> nodes)
    {
        if (formulaController.gameObject.activeInHierarchy && currentButton == button)
        {
            formulaController.gameObject.SetActive(false);
            return;
        }
        currentButton = button;
        currentValue = text;
        currentNodes = nodes;
        formulaController.gameObject.SetActive(true);
        if (nodes != null && nodes.Count != 0)
            formulaController.LoadFormula(nodes);
        else formulaController.Initialize();
    }

    protected void FormulaController_onSelectCell()
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
}
