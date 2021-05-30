using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/Ammeter/Ammeter")]

public class AmmeterInstruction : InstrumentBase
{
    public override string InstName => "电流表";

    public override double URV => 15;

    public override double LRV => -3;

    public override double ErrorLimit => 0.5; //忘了

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "安培";

    public override string UnitSymbol => "A";

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
        Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().ShowNum((float)value);
    }

    public override double GenMainValue()
    {
        return Random.Range((float)LRV, (float)URV);
    }

    public override void OnShow()
    {
        base.OnShow();
        ShowValue(GenMainValue());
    }
}
