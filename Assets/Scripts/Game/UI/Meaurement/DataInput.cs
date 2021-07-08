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
    public bool Inputable
    {
        get => _Value.readOnly;
        set => _Value.readOnly = value;
    }

    public string Value { get => _Value.text; set => _Value.text = value; }
    public int GroupNumber { get => Convert.ToInt32(_GroupNumber.text.Remove(_GroupNumber.text.Length - 1, 1)); 
        set => _GroupNumber.text = $"{value}."; }
    
    public void Show(int groupNumber)
    {
        GroupNumber = groupNumber;
    }

    public void Show(int groupNumber, string value)
    {
        Value = value;
        GroupNumber = groupNumber;
    }
}
