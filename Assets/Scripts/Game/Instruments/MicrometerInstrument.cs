using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/Micrometer/Micrometer")]
public class MicrometerInstrument : InstrumentBase
{
    public override string InstName { get => "螺旋测微器"; }

    public override double URV { get => 500; }

    public override double LRV { get => 0; }

    public override double ErrorLimit { get => 0.5; }

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit { get => "毫米"; }

    public override string UnitSymbol { get => "mm"; }

    public override GameObject gameObject { get; set; }

    public override Sprite previewImage { get => Resources.Load<Sprite>("Instruments/Micrometer/micrometer_preview"); }

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }

    public override void InstReset()
    {
        throw new System.NotImplementedException();
    }
}
