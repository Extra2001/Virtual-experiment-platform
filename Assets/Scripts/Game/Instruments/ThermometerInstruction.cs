using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[EntityResource(null, null, "Instruments/Thermometer/Thermometer")]
public class ThermometerInstruction : InstrumentBase
{
    public override string InstName  => "温度计";

    public override double URV => 50; 

    public override double LRV => -30; 

    public override double ErrorLimit => 0.5; //忘了

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit=> "摄氏度"; 

    public override string UnitSymbol=> "°C"; 

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
        throw new System.NotImplementedException();
    }
}
