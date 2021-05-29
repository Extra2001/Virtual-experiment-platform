using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[EntityResource(null, null, "Instruments/Voltmeter/Voltmeter")]

public class VoltmeterInstruction : InstrumentBase
{
    //启用自动化
    public override string InstName => "电压表";

    public override double URV => 15;

    public override double LRV => -3;

    public override double ErrorLimit => 0.5;//忘了

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "伏特";

    public override string UnitSymbol => "V";

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
