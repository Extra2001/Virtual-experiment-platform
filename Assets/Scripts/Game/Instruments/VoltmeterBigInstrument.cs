/************************************************************************************
    作者：张柯、张峻凡
    描述：电压表的数据和行为
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[EntityResource(null, null, "Instruments/Voltmeter/Voltmeter_big")]

public class VoltmeterBigInstrument : IndirectMeasurementInstrumentBase
{
    //override的变量和函数请参考基类InstrumentBase中的说明
    public override string InstName => "电压表（大量程）";

    public override double URV { get; set; } = 15;

    public override double LRV { get; set; } = -3;

    public override double ErrorLimit { get; set; } = 0.5;//1格最小刻度，为了相对明显一点

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "伏特";

    public override string UnitSymbol => "V";

    public override string previewImagePath => "Instruments/Voltmeter/Voltmeter";

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }
    public override bool ShowValue(double value, bool silent = false)
    {
        if (base.ShowValue(value, silent))
        {
            Entity.FindChildren("Voltmeter_son").GetComponent<RotateVoltmeter>().ShowNum((float)value);
            return true;
        }
        return false;
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
        ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit, true);
        Entity.FindChildren("Voltmeter_son").GetComponent<RotateVoltmeter>().MaxV = 15;
    }

    public override void ReshowValue()
    {
        ShowValue(MainValue + UnityEngine.Random.Range(-1f, 1f) * RandomErrorLimit, true);
    }


    public override void ShowGameButton(List<GameButtonItem> buttonItems)
    {
        base.ShowGameButton(buttonItems);
        buttonItems.Where(x => x.GameObject.name.Equals("CloseButton")).FirstOrDefault().OnClick.Add(() =>
        {
            Entity.FindChildren("Voltmeter_son").GetComponent<RotateVoltmeter>().UsingX();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("ResetButton")).FirstOrDefault().OnClick.Add(() =>
        {
            ReshowValue();
        });
    }
}
