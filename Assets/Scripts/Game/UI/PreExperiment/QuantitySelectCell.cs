using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuantitySelectCell : HTBehaviour
{
    public Text _Name;
    public Text _Symbol;

    public QuantityModel Quantity { get => _Quantity; set => SetQuantity(value); }

    private QuantityModel _Quantity = null;

    public void SetQuantity(QuantityModel model)
    {
        _Quantity = model;
        _Name.text = model.Name;
        _Symbol.text = model.Symbol;
    }
}
