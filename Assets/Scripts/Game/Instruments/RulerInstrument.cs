using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/Ruler/Ruler")]
public class RulerInstrument : InstrumentBase
{
    public override string InstName => "钢板尺"; 

    public override double URV { get; set; } = 500; 

    public override double LRV { get; set; } = 0; 

    public override double ErrorLimit { get; set; } = 0.5; 

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "毫米"; 

    public override string UnitSymbol  => "mm"; 

    public override string previewImagePath => "Instruments/Ruler/ruler_preview";

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
