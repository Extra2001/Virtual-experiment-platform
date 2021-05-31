using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[EntityResource(null, null, "Instruments/Voltmeter/Voltmeter")]

public class VoltmeterInstruction : InstrumentBase
{
    //启用自动化
    public override string InstName => "电压表";

    public override double URV { get; set; } = 3;

    public override double LRV { get; set; } = -1;

    private int RangeState = 0;//0代表小量程，1代表大量程

    public override double ErrorLimit { get; set; } = 0.5;//忘了

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "伏特";

    public override string UnitSymbol => "V";

    public override string previewImagePath => "Instruments/Ruler/ruler_preview";

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }
    public override void ShowValue(double value)
    {
        Entity.FindChildren("Voltmeter_son").GetComponent<RotateVoltmeter>().ShowNum((float)value);
    }

    public override void InstReset()
    {
        Entity.FindChildren("Voltmeter_son").GetComponent<RotateVoltmeter>().ShowNum(0.0f);
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
        Entity.FindChildren("Voltmeter_son").GetComponent<VAStateChange>().SwitchState1();
        RangeState = 0;
        URV = 3;
        LRV = -1;
    }

    public override void ShowInfoPanel(Dictionary<string, IntrumentInfoItem> infoItems)
    {
        base.ShowInfoPanel(infoItems);

        infoItems["_SwitchRange"].GameObject.SetActive(true);
        infoItems["_SwitchRange"].onValueChanged.Add(() =>
        {
            if (RangeState == 0)
            {
                URV = 15;
                LRV = -3;
                RangeState = 1;
                infoItems["_LRV"].GameObject.GetComponent<Text>().text = LRV.ToString();
                infoItems["_URV"].GameObject.GetComponent<Text>().text = URV.ToString();
                Entity.FindChildren("Voltmeter_son").GetComponent<VAStateChange>().SwitchState2();
                ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
            }
            else if (RangeState == 1)
            {
                URV = 3;
                LRV = -1;
                RangeState = 0;
                infoItems["_LRV"].GameObject.GetComponent<Text>().text = LRV.ToString();
                infoItems["_URV"].GameObject.GetComponent<Text>().text = URV.ToString();
                Entity.FindChildren("Voltmeter_son").GetComponent<VAStateChange>().SwitchState1();
                ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
            }
        });
    }

}
