/************************************************************************************
    作者：张柯、张峻凡
    描述：电子秤的数据和行为
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using System.Linq;

[EntityResource(null, null, "Instruments/ElectronicScales/ElectronicScales")]
public class ElectronicScalesInstrument : IndirectMeasurementInstrumentBase
{
    public override int ID => 5;
    //override的变量和函数请参考基类InstrumentBase中的说明
    public override string InstName => "电子秤";

    public override double URV { get; set; } = 2100;

    public override double LRV { get; set; } = 0;

    public int RangeState = 0;//0代表g,1还是代表g

    public override double ErrorLimit { get; set; } = 0.1;

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
    public override bool ShowValue(double value, bool silent = false)
    {
        if (base.ShowValue(value, silent))
        {
            Entity.FindChildren("ElectronicScales_son").GetComponent<ElectronicScales_main>().ShowNum((float)value);
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
            Entity.FindChildren("ElectronicScales_son").GetComponent<ElectronicScales_main>().UsingX();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("ResetButton")).FirstOrDefault().OnClick.Add(() =>
        {
            ReshowValue();
        });
    }
}
