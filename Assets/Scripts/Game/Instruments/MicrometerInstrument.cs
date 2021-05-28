using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/Micrometer/Micrometer")]
public class MicrometerInstrument : InstrumentBase
{
    public override string InstName => "螺旋测微器";

    public override double URV  => 500; 

    public override double LRV  => 0; 

    public override double ErrorLimit  => 0.5;
    
    public double ZeroPointError = 0.0f;//零点误差

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "毫米"; 

    public override string UnitSymbol => "mm"; 

    public override string previewImagePath => "Instruments/Micrometer/micrometer_preview";

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
