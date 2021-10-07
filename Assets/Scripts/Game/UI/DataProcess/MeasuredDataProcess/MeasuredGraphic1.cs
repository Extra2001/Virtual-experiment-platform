using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuredGraphic1 : HTBehaviour
{
    public InputField symbol;
    public Dropdown yaxis;
    public Dropdown graphicnextvalue;


    private QuantityModel quantity;

    private void Start()
    {
        symbol.onValueChanged.AddListener(input =>
        {
            quantity.selfSymbol = input;
        });

        yaxis.onValueChanged.AddListener(choice =>
        {
            quantity.Yaxis = choice;
        });

        graphicnextvalue.onValueChanged.AddListener(choice =>
        {
            quantity.graphicNextValue = choice;
        });
    }

    public void Show(QuantityModel quantity)
    {
        this.quantity = quantity;
        if (quantity.selfSymbol != null)
        {
            symbol.text = quantity.selfSymbol;
        }
    }

    public bool CheckAll(bool silent = false)
    {
        //������������Ƿ���ȷ
        if (quantity.IndependentData.data.Count != quantity.MesuredData.data.Count)
        {
            if (!silent)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "���������������ȷ"
                });
            }

            return false;
        }
        if (quantity.selfSymbol == "")
        {
            if (!silent)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "�������Ա�����λ"
                });
            }

            return false;
        }


        return true;
    }
}
