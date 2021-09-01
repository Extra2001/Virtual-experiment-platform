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

    public override double URV { get; set; } = 500;

    public override double LRV { get; set; } = 0;

    public override double ErrorLimit { get; set; } = 0.5;

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
            KeyboardManager.Keybd_event(88, 0, 0, 0);//模拟快捷键输入
        });
        buttonItems.Where(x => x.GameObject.name.Equals("OutwardButton")).FirstOrDefault().OnClick.Add(() =>
        {
            KeyboardManager.Keybd_event(79, 0, 0, 0);            
        });
        buttonItems.Where(x => x.GameObject.name.Equals("InwardButton")).FirstOrDefault().OnClick.Add(() =>
        {
            KeyboardManager.Keybd_event(80, 0, 0, 0);
        });
    }
}
