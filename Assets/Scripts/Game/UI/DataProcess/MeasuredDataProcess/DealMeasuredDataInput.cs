using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
        Value1.text = "=" + NumberFormat(quantity.Average);
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
        Value2.text = "=" + NumberFormat(quantity.Ua);
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
        Value3.text = "=" + NumberFormat(quantity.Ub);
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
        Field4.SetActive(true);
        if (quantity.AverageExpression != null)
        {

            Field1.GetComponent<FormulaController>().LoadFormula(quantity.AverageExpression);
        }
        else
        {
            Field1.GetComponent<FormulaController>().Initialize();
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
            Value1.text = "=" + NumberFormat(Field1.GetComponent<FormulaController>().ExpressionExecuted);
            CallButton1.image.sprite = Sprites[2];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].AverageState = 2;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].Average = Field1.GetComponent<FormulaController>().ExpressionExecuted;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].AverageExpression = Field1.GetComponent<FormulaController>().Serialize();
        }
        catch
        {
            //弹出报错提示框
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
            Value2.text = "=" + NumberFormat(Field2.GetComponent<FormulaController>().ExpressionExecuted);
            CallButton2.image.sprite = Sprites[2];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].UaState = 2;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].Ua = Field2.GetComponent<FormulaController>().ExpressionExecuted;
        }
        catch
        {
            //弹出报错提示框
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
            Value3.text = "=" +  NumberFormat(Field3.GetComponent<FormulaController>().ExpressionExecuted);
            CallButton3.image.sprite = Sprites[2];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].UbState = 2;
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].Ub = Field3.GetComponent<FormulaController>().ExpressionExecuted;
        }
        catch
        {
            //弹出报错提示框
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
            CallButton4.image.sprite = Sprites[2];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].ComplexState = 2;
        }
        catch
        {
            //弹出报错提示框
            CallButton4.image.sprite = Sprites[1];
            RecordManager.tempRecord.quantities[RecordManager.tempRecord.currentQuantityIndex].ComplexState = 1;
        }
    }


    //使double数据展示起来更好看
    public string NumberFormat(double Input)
    {
        string Output;

        if (Math.Abs(Input) > 0.01 && Math.Abs(Input) < 1000)
        {
            Output = Input.ToString("f4");
        }
        else if ((Input - 0) == 0)
        {
            Output = "0";
        }
        else
        {
            Output = Input.ToString("E");
        }

        return Output;
    }

}
