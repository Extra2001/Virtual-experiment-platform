using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UncertaintyInput : HTBehaviour
{
    public Text _Name;
    public Text _Symbol;
    public Text _Unit;
    public Text _Group;
    public Text _Instrument;
    public InputField UA;
    public InputField UB;
    public DataColumn dataColumn;

    private QuantityModel quantity;

    private void Start()
    {
        UA.onEndEdit.AddListener(x =>
        {
            quantity.UA = Convert.ToDouble(x);
        });
        UB.onEndEdit.AddListener(x =>
        {
            quantity.UB = Convert.ToDouble(x);
        });
    }

    public void Show(QuantityModel quantity)
    {
        this.quantity = quantity;
        _Name.text = quantity.Name;
        _Symbol.text = quantity.Symbol;
        var instance = (InstrumentBase)System.Activator.CreateInstance(quantity.InstrumentType);
        _Unit.text = instance.UnitSymbol;
        _Instrument.text = instance.InstName;
        UA.text = quantity.UA.ToString();
        UB.text = quantity.UB.ToString();
        dataColumn.ShowQuantity(quantity);
    }
}
