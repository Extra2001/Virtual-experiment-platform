using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InstrumentPanel : HTBehaviour
{
    public Image Image;
    public Text Title;
    public Text Range;
    public Text Unit;
    public Text Accuracy;
    public Button Generate;

    public BagSelector selector;
    public Type instrumentType;

    private void Start()
    {
        Generate.onClick.AddListener(() =>
        {
            selector.GenerateInstrument(this);
        });
    }

    private string double2string(double value)
    {
        var str = value.ToString();
        if (str.Contains("."))
            str = str.TrimEnd('0');
        if (str.EndsWith("."))
            str = str.Remove(str.Length - 1, 1);
        return str;
    }

    public void SetData(Type instrumentType)
    {
        this.instrumentType = instrumentType;

        var instance = instrumentType.CreateInstrumentInstance();
        Image.sprite = instance.previewImage;
        Title.text = instance.InstName;
        Range.text = double2string(instance.URV - instance.LRV);
        Unit.text = instance.Unit;
        Accuracy.text = double2string(instance.ErrorLimit);
    }
}
