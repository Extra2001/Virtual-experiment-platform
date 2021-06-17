using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[EntityResource(null, null, "Instruments/Micrometer/Micrometer")]
public class MicrometerInstrument : DirectMeasurementInstrumentBase
{
    public override string InstName => "螺旋测微器";

    public override double URV { get; set; } = 500; 

    public override double LRV { get; set; } = 0; 

    public override double ErrorLimit { get; set; } = 0.5;
    
    public double ZeroPointError = 0.0f;//零点误差

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "毫米"; 

    public override string UnitSymbol => "mm"; 

    public override string previewImagePath => "Instruments/Micrometer/micrometer_preview";

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
        throw new System.NotImplementedException();
    }

    public override void ShowGameButton(List<GameButtonItem> buttonItems)
    {
        base.ShowGameButton(buttonItems);
        buttonItems.Where(x => x.GameObject.name.Equals("CloseButton")).FirstOrDefault().OnClick.Add(() =>
        {
            KeyboardManager.Keybd_event(88, 0, 0, 0);
        });
        buttonItems.Where(x => x.GameObject.name.Equals("OutwardButton")).FirstOrDefault().OnClick.Add(() =>
        {
            KeyboardManager.Keybd_event(80, 0, 0, 0);
        });
        buttonItems.Where(x => x.GameObject.name.Equals("InwardButton")).FirstOrDefault().OnClick.Add(() =>
        {
            KeyboardManager.Keybd_event(79, 0, 0, 0);
        });
    }
}
