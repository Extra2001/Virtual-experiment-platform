using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuantityPreviewCell : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public Text _Name;
    public Text _Symbol;
    public Text _Group;
    public Text _InstrumentName;
    public Text _InstrumentUnit;

    public void SetQuantity(QuantityModel model)
    {
        _Name.text = model.Name;
        _Symbol.text = model.Symbol;
        _Group.text = model.Groups.ToString();
        var inst = (InstrumentBase)System.Activator.CreateInstance(model.InstrumentType);
        _InstrumentName.text = inst.Name;
        _InstrumentUnit.text = inst.UnitSymbol;
    }
}
