using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        Debug.Log(Field1.GetComponent<FormulaController>().Expression);
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
        //开始传值
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
        //开始传值
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
        //开始传值
    }

}
