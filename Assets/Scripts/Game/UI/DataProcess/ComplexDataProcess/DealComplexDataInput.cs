using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        CurrentField = Field1;

        StartShow();//加载存档中的情况

    }

    
    private void StartShow()
    {
        ComplexQuantityMoedel model = RecordManager.tempRecord.complexQuantityMoedel;

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

        CallButton3.image.sprite = Sprites[model.AnswerState];
        Field3.SetActive(true);
        if (model.AnswerExpression != null)
        {

            Field3.GetComponent<FormulaController>().LoadFormula(model.AnswerExpression);
        }
        else
        {
            Field3.GetComponent<FormulaController>().Initialize();
        }
        Field3.SetActive(false);
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
            RecordManager.tempRecord.complexQuantityMoedel.AverageState = 2;
            RecordManager.tempRecord.complexQuantityMoedel.Average = Field1.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.complexQuantityMoedel.AverageExpression = Field1.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
            CallButton1.image.sprite = Sprites[1];
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
            RecordManager.tempRecord.complexQuantityMoedel.UncertainState = 2;
            RecordManager.tempRecord.complexQuantityMoedel.Uncertain = Field2.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.complexQuantityMoedel.UncertainExpression = Field2.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
            CallButton2.image.sprite = Sprites[1];
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
            //~~~~~
            CallButton3.image.sprite = Sprites[2];
        }
        catch
        {
            //弹出报错提示框
            CallButton3.image.sprite = Sprites[1];
        }
    }

}
