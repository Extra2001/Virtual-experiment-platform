using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

[EntityResource(null, null, "Instruments/ElectronicScales/ElectronicScales")]
public class ElectronicScalesInstrument : IndirectMeasurementInstrumentBase
{
    public override string InstName => "电子秤";

    public override double URV { get; set; } = 9999;

    public override double LRV { get; set; } = 0;

    public int RangeState = 0;//0代表g,1还是代表g

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
        if (value < 0)
        {
            value = 0;
        }else if (value > 9999)
        {
            value = 9999;
        }
        Entity.FindChildren("ElectronicScales_son").GetComponent<ElectronicScales_main>().ShowNum((float)value);

    }

    public override void InstReset()
    {
        throw new System.NotImplementedException();
    }

    public override void GenMainValueAndRandomErrorLimit()
    {
        RandomErrorLimit = ErrorLimit * UnityEngine.Random.Range(-1f, 1f);
    }

    public override void OnShow()
    {
        //Entity.FindChildren("ElectronicScales_son").GetComponent<ElectronicScales_main>().ShowNum(1000f);
        base.OnShow();
        GenMainValueAndRandomErrorLimit();
        ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
    }

    public override void ReshowValue()
    {
        GenMainValueAndRandomErrorLimit();
        ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
    }

    public override void ShowGameButton(List<GameButtonItem> buttonItems)
    {
        base.ShowGameButton(buttonItems);
        buttonItems.Where(x => x.GameObject.name.Equals("CloseButton")).FirstOrDefault().OnClick.Add(() =>
        {
            KeyboardManager.Keybd_event(88, 0, 0, 0);
        });
        buttonItems.Where(x => x.GameObject.name.Equals("ResetButton")).FirstOrDefault().OnClick.Add(() =>
        {
            ReshowValue();
        });
    }
}
