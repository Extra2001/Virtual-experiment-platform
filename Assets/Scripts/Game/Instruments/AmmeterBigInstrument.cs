/************************************************************************************
    作者：张峻凡、张柯
    描述：电流表数据和行为
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

[EntityResource(null, null, "Instruments/Ammeter/Ammeter_big")]

public class AmmeterBigInstrument : IndirectMeasurementInstrumentBase
{
    //override的变量和函数请参考基类InstrumentBase中的说明
    public override string InstName => "电流表（大量程）";

    public override double URV { get; set; } = 3;
    public override double LRV { get; set; } = -1;

    public override double ErrorLimit { get; set; } = 0.04; //4格最小刻度，为了相对明显一点

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "安培";

    public override string UnitSymbol => "A";

    public override string previewImagePath => "Instruments/Ammeter/ammeter";

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }

    public override void ShowValue(double value)
    {
        Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().ShowNum((float)value);
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
        Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().MaxA = 3f;
    }

    public override void ReshowValue()
    {
        ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
    }

    public void SwitchRange(double _MainValue)
    {
        Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().MaxA = 3f;
        MainValue = _MainValue;
        RandomErrorLimit = ErrorLimit * UnityEngine.Random.Range(-1f, 1f);
        ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
    }

    public override void ShowInfoPanel(Dictionary<string, IntrumentInfoItem> infoItems)
    {
        //电压表和电流表还具有额外的切换量程功能
        base.ShowInfoPanel(infoItems);
        infoItems["_SwitchRange"].GameObject.SetActive(true);
        infoItems["_SwitchRange"].onValueChanged.Add(() =>
        {
            /*if (RangeState == 0)
            {
                URV = 3;
                LRV = -1;
                ErrorLimit = 0.2;
                RangeState = 1;
                infoItems["_LRV"].GameObject.GetComponent<Text>().text = LRV.ToString();
                infoItems["_URV"].GameObject.GetComponent<Text>().text = URV.ToString();
                Entity.FindChildren("Ammeter_son").GetComponent<VAStateChange>().SwitchState2();
                Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().MaxA = 3f;
                ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
            }
            else if (RangeState == 1)
            {
                URV = 0.6;
                LRV = -0.2;
                ErrorLimit = 0.04;
                RangeState = 0;
                infoItems["_LRV"].GameObject.GetComponent<Text>().text = LRV.ToString();
                infoItems["_URV"].GameObject.GetComponent<Text>().text = URV.ToString();
                Entity.FindChildren("Ammeter_son").GetComponent<VAStateChange>().SwitchState1();
                Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().MaxA = 0.6f;
                ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
            }*/
        });
    }

    public override void ShowGameButton(List<GameButtonItem> buttonItems)
    {
        base.ShowGameButton(buttonItems);
        buttonItems.Where(x => x.GameObject.name.Equals("CloseButton")).FirstOrDefault().OnClick.Add(() =>
        {
            Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().UsingX();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("ResetButton")).FirstOrDefault().OnClick.Add(() =>
        {
            ReshowValue();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("SwitchButton")).FirstOrDefault().OnClick.Add(() =>
        {
            /*if (RangeState == 0)
            {
                URV = 3;
                LRV = -1;
                ErrorLimit = 0.2;
                RangeState = 1;
                Entity.FindChildren("Ammeter_son").GetComponent<VAStateChange>().SwitchState2();
                Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().MaxA = 3f;
                ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
            }
            else if (RangeState == 1)
            {
                URV = 0.6;
                LRV = -0.2;
                ErrorLimit = 0.04;
                RangeState = 0;
                Entity.FindChildren("Ammeter_son").GetComponent<VAStateChange>().SwitchState1();
                Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().MaxA = 0.6f;
                ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit);
            }*/
        });
    }
}
 