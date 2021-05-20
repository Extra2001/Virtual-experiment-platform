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

    public Sprite StartSprite;
    public Sprite WorkingSprite;
    public Sprite EndSprite;
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
            CallButton1.image.sprite = EndSprite;
        }
        catch
        {
            //弹出报错提示框
            CallButton1.image.sprite = WorkingSprite;
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
            CallButton2.image.sprite = EndSprite;
        }
        catch
        {
            //弹出报错提示框
            CallButton2.image.sprite = WorkingSprite;
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
            CallButton3.image.sprite = EndSprite;
        }
        catch
        {
            //弹出报错提示框
            CallButton3.image.sprite = WorkingSprite;
        }
    }


    private string NumberFormat(double Input)
    {
        string Output;

        if (Input > 0.01 && Input < 1000)
        {
            Output = Input.ToString("f4");
        }
        else
        {
            Output = Input.ToString("E");
        }

        return Output;
    }
}