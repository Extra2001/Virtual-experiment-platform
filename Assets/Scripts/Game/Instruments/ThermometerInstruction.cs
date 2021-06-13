using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
[EntityResource(null, null, "Instruments/Thermometer/Thermometer")]
public class ThermometerInstruction : IndirectMeasurementInstrumentBase
{
    public override string InstName  => "温度计";

    public override double URV { get; set; } = 50; 

    public override double LRV { get; set; } = -30;

    public override double ErrorLimit { get; set; } = 0.5; //忘了

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit=> "摄氏度"; 

    public override string UnitSymbol=> "℃"; 

    public override string previewImagePath => "Instruments/Thermometer/thermometer_preview";

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }

    public override void InstReset()
    {
        throw new System.NotImplementedException();
    }

    public override void ShowValue(double value)
    {
        Entity.FindChildren("Thermometer_son").GetComponent<thermometer_main>().ShowNum((float)value);
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
        ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
    }

}
