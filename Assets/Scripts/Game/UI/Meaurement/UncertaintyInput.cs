using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UncertaintyInput : HTBehaviour
{
    public Text _Title;
    public Text _AverageTitle;
    public Text _AverageValue;
    public Text _UaTitle;
    public Text _UaValue;
    public Text _UbTitle;
    public Text _UbValue;
    public Text _ComplexTitle;

    public DataColumn dataColumn;


    private void Start()
    {

    }

    public void Show(QuantityModel quantity)
    {
        var instance = quantity.InstrumentType.CreateInstrumentInstance();

        _Title.text = "处理" + quantity.Name + ":" + quantity.Symbol + "/" + instance.UnitSymbol;
        _AverageTitle.text = instance.UnitSymbol + "的平均值=";
        _UaTitle.text = instance.UnitSymbol + "的A类不确定度=";
        _UbTitle.text = instance.UnitSymbol + "的B类不确定度=";
        _ComplexTitle.text = instance.UnitSymbol + "的合成不确定度=";

        dataColumn.ShowQuantity(quantity);
    }
}
