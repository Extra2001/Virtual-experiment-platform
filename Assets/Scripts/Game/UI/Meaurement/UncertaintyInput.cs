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

        _Title.text = "����" + quantity.Name + ":" + quantity.Symbol + "/" + instance.UnitSymbol;
        _AverageTitle.text = instance.UnitSymbol + "��ƽ��ֵ=";
        _UaTitle.text = instance.UnitSymbol + "��A�಻ȷ����=";
        _UbTitle.text = instance.UnitSymbol + "��B�಻ȷ����=";
        _ComplexTitle.text = instance.UnitSymbol + "�ĺϳɲ�ȷ����=";

        dataColumn.ShowQuantity(quantity);
    }
}
