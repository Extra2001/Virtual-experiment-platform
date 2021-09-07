/************************************************************************************
    作者：张柯、张峻凡
    描述：螺旋测微器的数据和行为
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using System.Linq;

[EntityResource(null, null, "Instruments/Micrometer/Micrometer")]
public class MicrometerInstrument : DirectMeasurementInstrumentBase
{
    //override的变量和函数请参考基类InstrumentBase中的说明
    public override string InstName => "螺旋测微器";

    public override double URV { get; set; } = 10; 

    public override double LRV { get; set; } = 0; 

    public override double ErrorLimit { get; set; } = 0.005;
    
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

    public override void ShowGameButton(List<GameButtonItem> buttonItems)
    {
        base.ShowGameButton(buttonItems);
        buttonItems.Where(x => x.GameObject.name.Equals("CloseButton")).FirstOrDefault().OnClick.Add(() =>
        {
            Entity.FindChildren("Micrometer_son").GetComponent<Micrometer_main>().UsingX();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("OutwardButton")).FirstOrDefault().OnClick.Add(() =>
        {
            Entity.FindChildren("Micrometer_son").GetComponent<Micrometer_main>().UsingO();
        });
        buttonItems.Where(x => x.GameObject.name.Equals("InwardButton")).FirstOrDefault().OnClick.Add(() =>
        {
            Entity.FindChildren("Micrometer_son").GetComponent<Micrometer_main>().UsingP();
        });
    }
}
