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

        _Title.text = "����" + quantity.Name + ":" + quantity.Symbol + "/" + instance.UnitSymbol;
        _AverageTitle.text = quantity.Name + "��ƽ��ֵ\n\n=";
        _UaTitle.text = quantity.Name + "��A�಻ȷ����\n\n=";
        _UbTitle.text = quantity.Name + "��B�಻ȷ����\n\n=";
        _ComplexTitle.text = quantity.Name + "�ĺϳɲ�ȷ����\n\n=";

        dataColumn.ShowQuantity(quantity, true);
    }
}
