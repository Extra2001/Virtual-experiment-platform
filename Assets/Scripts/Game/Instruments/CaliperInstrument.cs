/************************************************************************************
    作者：张峻凡、张柯
    描述：游标卡尺数据和行为
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[EntityResource(null, null, "Instruments/Caliper/Caliper")]
public class CaliperInstrument : DirectMeasurementInstrumentBase
{
    //override的变量和函数请参考基类InstrumentBase中的说明
    public override string InstName => "游标卡尺";

    public override double URV { get; set; } = 250;

    public override double LRV { get; set; } = 0;

    public override double ErrorLimit { get; set; } = 0.02;

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "毫米";

    public override string UnitSymbol => "mm";

    public override string previewImagePath => "Instruments/Caliper/caliper_preview";

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }

    public override void ShowGameButton(List<GameButtonItem> buttonItems)
    {
        base.ShowGameButton(buttonItems);
        buttonItems.Where(x => x.GameObject.name.Equals("CloseButton")).FirstOrDefault().OnClick.Add(() =>
        {
            Entity.FindChildren("Caliper_son").GetComponent<Caliper_main>().UsingX();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("OutwardButton")).FirstOrDefault().OnClick.Add(() =>
        {
            Entity.FindChildren("Caliper_son").GetComponent<Caliper_main>().UsingO();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("InwardButton")).FirstOrDefault().OnClick.Add(() =>
        {
            Entity.FindChildren("Caliper_son").GetComponent<Caliper_main>().UsingP();
        });
    }
}
