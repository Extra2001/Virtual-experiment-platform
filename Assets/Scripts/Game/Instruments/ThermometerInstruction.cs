using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
[EntityResource(null, null, "Instruments/Thermometer/Thermometer")]
public class ThermometerInstruction : InstrumentBase
{
    public override string InstName  => "温度计";

    public override double URV { get; set; } = 50; 

    public override double LRV { get; set; } = -30;

    private int RangeState = 0;//0代表C，1代表F

    public override double ErrorLimit { get; set; } = 0.5; //忘了

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit=> "摄氏度"; 

    public override string UnitSymbol=> "℃"; 

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
        Entity.FindChildren("Thermometer_son").GetComponent<thermometer_main>().ShowNum((float)value);
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
        Entity.FindChildren("Thermometer_son").GetComponent<CFStateChange>().SwitchState1();
        Entity.FindChildren("Thermometer_son").GetComponent<thermometer_main>().IsC = true;
        RangeState = 0;
        URV = 50;
        LRV = -30;
    }

    public override void ShowInfoPanel(Dictionary<string, IntrumentInfoItem> infoItems)
    {
        base.ShowInfoPanel(infoItems);
        infoItems["_SwitchRange"].GameObject.SetActive(true);
        infoItems["_SwitchRange"].onValueChanged.Add(() =>
        {
            if (RangeState == 0)
            {
                URV = 50;
                LRV = -30;
                RangeState = 1;
                infoItems["_LRV"].GameObject.GetComponent<Text>().text = LRV.ToString();
                infoItems["_URV"].GameObject.GetComponent<Text>().text = URV.ToString();
                infoItems["_Unit"].GameObject.GetComponent<Text>().text = "摄氏度";
                infoItems["_UnitSymbol"].GameObject.GetComponent<Text>().text = "℃";
                Entity.FindChildren("Thermometer_son").GetComponent<CFStateChange>().SwitchState1();
                Entity.FindChildren("Thermometer_son").GetComponent<thermometer_main>().IsC = true;
                ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
            }
            else if (RangeState == 1)
            {
                URV =122;
                LRV = -22;
                RangeState = 0;
                infoItems["_LRV"].GameObject.GetComponent<Text>().text = LRV.ToString();
                infoItems["_URV"].GameObject.GetComponent<Text>().text = URV.ToString();
                infoItems["_Unit"].GameObject.GetComponent<Text>().text = "华氏度";
                infoItems["_UnitSymbol"].GameObject.GetComponent<Text>().text = "℉";
                Entity.FindChildren("Thermometer_son").GetComponent<CFStateChange>().SwitchState2();
                Entity.FindChildren("Thermometer_son").GetComponent<thermometer_main>().IsC = false;
                ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
            }
        });
    }
}
