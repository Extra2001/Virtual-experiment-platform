/************************************************************************************
    作者：张峻凡、张柯
    描述：电流表数据和行为
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

[EntityResource(null, null, "Instruments/Ammeter/Ammeter_small")]

public class AmmeterSmallInstrument : IndirectMeasurementInstrumentBase
{
    //override的变量和函数请参考基类InstrumentBase中的说明
    public override string InstName => "电流表（小量程）";

    public override double URV { get; set; } = 0.6;
    public override double LRV { get; set; } = -0.2;

    public override double ErrorLimit { get; set; } = 0.02; //1格最小刻度，为了相对明显一点

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "安培";

    public override string UnitSymbol => "A";

    public override string previewImagePath => "Instruments/Ammeter/ammeter";

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }

    public override bool ShowValue(double value, bool silent = false)
    {
        if (base.ShowValue(value, silent))
        {
            Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().ShowNum((float)value);
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
        Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().MaxA = 0.6f;
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
            Entity.FindChildren("Ammeter_son").GetComponent<RotateAmmeter>().UsingX();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("ResetButton")).FirstOrDefault().OnClick.Add(() =>
        {
            ReshowValue();
        });
    }
}
 