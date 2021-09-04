/************************************************************************************
    作者：张峻凡
    描述：处理合成量的数据输入
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using UnityEngine.UI;

public class DealComplexDataInput : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public GameObject Formula;

    public Button CallButton1;
    public Button SureButton1;
    public GameObject Field1;
    public Text Value1;

    public Button CallButton2;
    public Button SureButton2;
    public GameObject Field2;
    public Text Value2;

    public Button CallButton3;
    public Button SureButton3;
    public GameObject Field3;

    public Text Value34;

    public Button CallButton4;
    public Button SureButton4;
    public GameObject Field4;


    public Sprite[] Sprites = new Sprite[3]; //0为初始，1为编辑中，2为编辑完成
    private GameObject CurrentField;

    // Start is called before the first frame update
    void Start()
    {
        CallButton1.onClick.AddListener(CallClick1);
        SureButton1.onClick.AddListener(SureClick1);
        CallButton2.onClick.AddListener(CallClick2);
        SureButton2.onClick.AddListener(SureClick2);
        CallButton3.onClick.AddListener(CallClick3);
        SureButton3.onClick.AddListener(SureClick3);
        CallButton4.onClick.AddListener(CallClick4);
        SureButton4.onClick.AddListener(SureClick4);
        CurrentField = Field1;        

        
    }

    
    public void StartShow()
    {
        LatexEquationRender.Render(RecordManager.tempRecord.stringExpression, res=> {
            Formula.GetComponent<Image>().FitHeight(res);
        });

        ComplexQuantityModel model = RecordManager.tempRecord.complexQuantityModel;

        CallButton1.image.sprite = Sprites[model.AverageState];
        Value1.text = "=" + StaticMethods.NumberFormat(model.Average);
        Field1.SetActive(true);
        if (model.AverageExpression != null)
        {

            Field1.GetComponent<FormulaController>().LoadFormula(model.AverageExpression);
        }
        else
        {
            Field1.GetComponent<FormulaController>().Initialize();
        }
        Field1.SetActive(false);

        CallButton2.image.sprite = Sprites[model.UncertainState];
        Value2.text = "=" + StaticMethods.NumberFormat(model.Uncertain);
        Field2.SetActive(true);
        if (model.UncertainExpression != null)
        {

            Field2.GetComponent<FormulaController>().LoadFormula(model.UncertainExpression);
        }
        else
        {
            Field2.GetComponent<FormulaController>().Initialize();
        }
        Field2.SetActive(false);

        CallButton3.image.sprite = Sprites[model.AnswerAverageState];
        Value34.text = "=" + StaticMethods.NumberFormat(model.AnswerAverage) + "±" + StaticMethods.NumberFormat(model.AnswerUncertain);
        Field3.SetActive(true);
        if (model.AnswerAverageExpression != null)
        {

            Field3.GetComponent<FormulaController>().LoadFormula(model.AnswerAverageExpression);
        }
        else
        {
            Field3.GetComponent<FormulaController>().Initialize();
        }
        Field3.SetActive(false);

        CallButton4.image.sprite = Sprites[model.AnswerUncertainState];
        Field4.SetActive(true);
        if (model.AnswerUncertainExpression != null)
        {

            Field4.GetComponent<FormulaController>().LoadFormula(model.AnswerUncertainExpression);
        }
        else
        {
            Field4.GetComponent<FormulaController>().Initialize();
        }
        Field4.SetActive(false);
    }

    private void CallClick1()
    {
        if (CurrentField == Field1)
        {
            if (CurrentField.activeInHierarchy == true)
            {
                CurrentField.SetActive(false);
            }
            else
            {
                CurrentField.SetActive(true);
            }
        }
        else
        {
            CurrentField.SetActive(false);
            Field1.SetActive(true);
            CurrentField = Field1;
        }

    }
    private void SureClick1()
    {
        try
        {
            Value1.text = "=" + StaticMethods.NumberFormat(Field1.GetComponent<FormulaController>().ExpressionExecuted);
            CallButton1.image.sprite = Sprites[2];
            RecordManager.tempRecord.complexQuantityModel.AverageState = 2;
            RecordManager.tempRecord.complexQuantityModel.Average = Field1.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.complexQuantityModel.AverageExpression = Field1.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
            ShowModel($"输入公式无法求解，请重新输入");

            CallButton1.image.sprite = Sprites[1];
            RecordManager.tempRecord.complexQuantityModel.AverageState = 1;
        }

    }
    private void CallClick2()
    {
        if (CurrentField == Field2)
        {
            if (CurrentField.activeInHierarchy == true)
            {
                CurrentField.SetActive(false);
            }
            else
            {
                CurrentField.SetActive(true);
            }
        }
        else
        {
            CurrentField.SetActive(false);
            Field2.SetActive(true);
            CurrentField = Field2;
        }
    }
    private void SureClick2()
    {
        try
        {
            Value2.text = "=" + StaticMethods.NumberFormat(Field2.GetComponent<FormulaController>().ExpressionExecuted);
            CallButton2.image.sprite = Sprites[2];
            RecordManager.tempRecord.complexQuantityModel.UncertainState = 2;
            RecordManager.tempRecord.complexQuantityModel.Uncertain = Field2.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.complexQuantityModel.UncertainExpression = Field2.GetComponent<FormulaController>().Serialize();
            Debug.LogWarning(Field2.GetComponent<FormulaController>().Expression);
        }
        catch
        {
            //弹出报错提示框
            ShowModel($"输入公式无法求解，请重新输入");

            CallButton2.image.sprite = Sprites[1];
            RecordManager.tempRecord.complexQuantityModel.UncertainState = 1;
        }
    }

    private void CallClick3()
    {
        if (CurrentField == Field3)
        {
            if (CurrentField.activeInHierarchy == true)
            {
                CurrentField.SetActive(false);
            }
            else
            {
                CurrentField.SetActive(true);
            }
        }
        else
        {
            CurrentField.SetActive(false);
            Field3.SetActive(true);
            CurrentField = Field3;
        }
    }
    private void SureClick3()
    {
        try
        {
            //检查最终结果表达式是否正确
            Value34.text = "=" + StaticMethods.NumberFormat(Field3.GetComponent<FormulaController>().ExpressionExecuted) + "±" + StaticMethods.NumberFormat(RecordManager.tempRecord.complexQuantityModel.AnswerUncertain);
            CallButton3.image.sprite = Sprites[2];
            RecordManager.tempRecord.complexQuantityModel.AnswerAverageState = 2;
            RecordManager.tempRecord.complexQuantityModel.AnswerAverage = Field3.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.complexQuantityModel.AnswerAverageExpression = Field3.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
            ShowModel($"输入公式无法求解，请重新输入");
            CallButton3.image.sprite = Sprites[1];
            RecordManager.tempRecord.complexQuantityModel.AnswerAverageState = 1;
        }
    }

    private void CallClick4()
    {
        if (CurrentField == Field4)
        {
            if (CurrentField.activeInHierarchy == true)
            {
                CurrentField.SetActive(false);
            }
            else
            {
                CurrentField.SetActive(true);
            }
        }
        else
        {
            CurrentField.SetActive(false);
            Field4.SetActive(true);
            CurrentField = Field4;
        }
    }
    private void SureClick4()
    {
        try
        {
            //检查最终结果表达式是否正确
            Value34.text = "=" + StaticMethods.NumberFormat(RecordManager.tempRecord.complexQuantityModel.AnswerAverage) + "±" + StaticMethods.NumberFormat(Field4.GetComponent<FormulaController>().ExpressionExecuted);
            CallButton4.image.sprite = Sprites[2];
            RecordManager.tempRecord.complexQuantityModel.AnswerUncertainState = 2;
            RecordManager.tempRecord.complexQuantityModel.AnswerUncertain = Field4.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.complexQuantityModel.AnswerUncertainExpression = Field4.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
            ShowModel($"输入公式无法求解，请重新输入");
            CallButton4.image.sprite = Sprites[1];
            RecordManager.tempRecord.complexQuantityModel.AnswerUncertainState = 1;
        }
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
