using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataInput : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public Text _GroupNumber;
    public InputField _Value;

    public double Value { get => Convert.ToDouble(_Value.text); set => _Value.text = value.ToString(); }
    public int GroupNumber { get => Convert.ToInt32(_GroupNumber.text.Remove(_GroupNumber.text.Length - 1, 1)); 
        set => _GroupNumber.text = $"{value}."; }
    
    public void Show(int groupNumber)
    {
        GroupNumber = groupNumber;
    }

    public void Show(int groupNumber, double value)
    {
        Value = value;
        GroupNumber = groupNumber;
    }
}
