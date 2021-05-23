using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/Caliper/Caliper")]
public class CaliperInstrument : InstrumentBase
{
    public override string InstName => "游标卡尺";

    public override double URV => 500;

    public override double LRV => 0;

    public override double ErrorLimit => 0.5;

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "毫米";

    public override string UnitSymbol => "mm";

    public override string previewImagePath => "Instruments/Caliper/caliper_preview";

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
