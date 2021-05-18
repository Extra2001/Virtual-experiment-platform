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
    public Text _UaTitle;
    public Text _UbTitle;
    public Text _ComplexTitle;

    public DataColumn dataColumn;


    private void Start()
    {
        
    }

    public void Show(QuantityModel quantity)
    {
        var instance = quantity.InstrumentType.CreateInstrumentInstance();

        _Title.text = "处理" + quantity.Name + ":" + quantity.Symbol + "/" + instance.UnitSymbol;
        _AverageTitle.text = quantity.Name + "的平均值\n\n=";
        _UaTitle.text = quantity.Name + "的A类不确定度\n\n=";
        _UbTitle.text = quantity.Name + "的B类不确定度\n\n=";
        _ComplexTitle.text = quantity.Name + "的合成不确定度\n\n=";

        dataColumn.ShowQuantity(quantity, true);
    }
}
