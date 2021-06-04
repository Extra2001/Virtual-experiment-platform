using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[EntityResource(null, null, "Instruments/ElectronicScales/ElectronicScales")]
public class ElectronicScalesInstrument : InstrumentBase
{
    public override string InstName => "电子秤";

    public override double URV { get; set; } = 9999999;

    public override double LRV { get; set; } = 0;

    private int RangeState = 0;//0代表g,1还是代表g

    public override double ErrorLimit { get; set; } = 0.5; //忘了

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit=> "克";

    public override string UnitSymbol => "g";

    public override string previewImagePath => "Instruments/ElectronicScales/ElectronicScales_image";

    private int accuracy_rating = 3;//显示小数点后三位

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }
    public override void ShowValue(double value)
    {

    }

    public override void InstReset()
    {
        throw new System.NotImplementedException();
    }

    public override void GenMainValueAndRandomErrorLimit()
    {
        MainValue = UnityEngine.Random.Range((float)LRV, (float)URV);
        RandomErrorLimit = ErrorLimit * UnityEngine.Random.Range(-1f, 1f);
    }

    public override void OnShow()
    {
        base.OnShow();
        GenMainValueAndRandomErrorLimit();
    }

    
}
