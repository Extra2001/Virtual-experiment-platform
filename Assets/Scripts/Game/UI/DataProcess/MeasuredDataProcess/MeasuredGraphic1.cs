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
        if (quantity.IndependentData == null) quantity.IndependentData = new DataColumnModel()
        {
            name = $"[自] 自变量 {quantity.Name}",
            quantitySymbol = quantity.Symbol,
            type = DataColumnType.Independent
        };
        yaxis.options[0].text = quantity.MesuredData.name;
        yaxis.options[1].text = quantity.IndependentData.name;
        if (quantity.selfSymbol != null)
        {
            symbol.text = quantity.selfSymbol;
        }
    }

    public bool CheckAll(bool silent = false)
    {
        //检查数据组数是否正确
        if (quantity.IndependentData.data.Count != quantity.MesuredData.data.Count)
        {
            if (!silent)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "你的数据组数不正确"
                });
            }

            return false;
        }

        if(StaticMethods.CheckIfExistNullOrEmpty(quantity.MesuredData.data) && StaticMethods.CheckIfExistNullOrEmpty(quantity.IndependentData.data))
        {
            if (!silent)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "数据填写未完成"
                });
            }

            return false;
        }


        if (string.IsNullOrEmpty(quantity.selfSymbol))
        {
            if (!silent)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "请输入自变量单位"
                });
            }

            return false;
        }


        return true;
    }
}
