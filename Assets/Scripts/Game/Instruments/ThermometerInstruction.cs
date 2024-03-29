/************************************************************************************
    作者：张柯、张峻凡
    描述：温度计的数据和行为
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using System.Linq;

[EntityResource(null, null, "Instruments/Thermometer/Thermometer")]
public class ThermometerInstruction : IndirectMeasurementInstrumentBase
{
    public override int ID => 4;
    //override的变量和函数请参考基类InstrumentBase中的说明
    public override string InstName  => "温度计";

    public override double URV { get; set; } = 50; 

    public override double LRV { get; set; } = -30;

    public override double ErrorLimit { get; set; } = 1;

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit=> "摄氏度"; 

    public override string UnitSymbol=> "℃"; 

    public override string previewImagePath => "Instruments/Thermometer/thermometer_preview";

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }

    public override bool ShowValue(double value, bool silent = false)
    {
        if (base.ShowValue(value, silent))
        {
            Entity.FindChildren("Thermometer_son").GetComponent<thermometer_main>().ShowNum((float)value);
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
            Entity.FindChildren("Thermometer_son").GetComponent<thermometer_main>().UsingX();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("ResetButton")).FirstOrDefault().OnClick.Add(() =>
        {
            ReshowValue();
        });
    }
}
