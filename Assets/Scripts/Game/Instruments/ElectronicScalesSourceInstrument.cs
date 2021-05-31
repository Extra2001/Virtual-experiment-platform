using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/ElectronicScales/ElectronicScalesSource")]
public class ElectronicScalesSourceInstrument : InstrumentBase
{
    [ObjectPath("ElectronicScalesSource_son/num")]
    private GameObject Num;

    public override string InstName => "电子秤";

    public override double URV { get; set; } = 9999999;

    public override double LRV { get; set; } = 0;

    public override double ErrorLimit { get; set; } = 0.5; //忘了

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit=> "克";

    public override string UnitSymbol => "g";

    public override string previewImagePath => "Instruments/ElectronicScales/ElectronicScales_image";

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
        Num.GetComponent<manager_num>().qwq = MainValue;
    }

    public override void OnShow()
    {
        AddRightButton();
        Entity.transform.GetChild(0).gameObject.SetActive(true);
        MainValue = Random.Range((float)LRV, (float)URV);
        ShowValue(MainValue);
        base.OnShow();
    }
}
