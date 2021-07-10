/************************************************************************************
    作者：张峻凡
    描述：处理直接测量量的数据输入
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using UnityEngine.UI;

public class DealMeasuredDataInput : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

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
    public Text Value3;

    public Button CallButton4;
    public Button SureButton4;
    public GameObject Field4;
    public Text Value4;

    public Sprite[] Sprites = new Sprite[3];
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

    public void Show(QuantityModel quantity)
    {
        //按钮颜色，存储的表达式等等

        CallButton1.image.sprite = Sprites[quantity.AverageState];
        Value1.text = "=" + StaticMethods.NumberFormat(quantity.Average);
        Field1.SetActive(true);
        if (quantity.AverageExpression != null)
        {
            
            Field1.GetComponent<FormulaController>().LoadFormula(quantity.AverageExpression);            
        }
        else
        {
            Field1.GetComponent<FormulaController>().Initialize();
        }
        Field1.SetActive(false);

        CallButton2.image.sprite = Sprites[quantity.UaState];
        Value2.text = "=" + StaticMethods.NumberFormat(quantity.Ua);
        Field2.SetActive(true);
        if (quantity.UaExpression != null)
        {

            Field2.GetComponent<FormulaController>().LoadFormula(quantity.UaExpression);
        }
        else
        {
            Field2.GetComponent<FormulaController>().Initialize();
        }
        Field2.SetActive(false);

        CallButton3.image.sprite = Sprites[quantity.UbState];
        Value3.text = "=" + StaticMethods.NumberFormat(quantity.Ub);
        Field3.SetActive(true);
        if (quantity.UbExpression != null)
        {

            Field3.GetComponent<FormulaController>().LoadFormula(quantity.UbExpression);
        }
        else
        {
            Field3.GetComponent<FormulaController>().Initialize();
        }
        Field3.SetActive(false);

        CallButton4.image.sprite = Sprites[quantity.ComplexState];
        Value4.text = "=" + StaticMethods.NumberFormat(quantity.Uncertain);
        Field4.SetActive(true);
        if (quantity.ComplexExpression != null)
        {

            Field4.GetComponent<FormulaController>().LoadFormula(quantity.ComplexExpression);
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
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].AverageState = 2;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].Average = Field1.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].AverageExpression = Field1.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
            ShowModel($"输入公式无法求解，请重新输入");
            CallButton1.image.sprite = Sprites[1];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].AverageState = 1;
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
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].UaState = 2;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].Ua = Field2.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].UaExpression = Field2.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
            ShowModel($"输入公式无法求解，请重新输入");
            CallButton2.image.sprite = Sprites[1];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].UaState = 1;
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
            Value3.text = "=" + StaticMethods.NumberFormat(Field3.GetComponent<FormulaController>().ExpressionExecuted);
            CallButton3.image.sprite = Sprites[2];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].UbState = 2;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].Ub = Field3.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].UbExpression = Field3.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
            ShowModel($"输入公式无法求解，请重新输入");
            CallButton3.image.sprite = Sprites[1];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].UbState = 1;
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
            //检查不确定度表达式是否正确
            //~~~~~
            Value4.text = "=" + StaticMethods.NumberFormat(Field4.GetComponent<FormulaController>().ExpressionExecuted);
            CallButton4.image.sprite = Sprites[2];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].ComplexState = 2;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].Uncertain = Field4.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].ComplexExpression = Field4.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
            ShowModel($"输入公式无法求解，请重新输入");
            CallButton4.image.sprite = Sprites[1];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].ComplexState = 1;
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
