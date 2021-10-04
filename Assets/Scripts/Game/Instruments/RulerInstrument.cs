/************************************************************************************
    作者：张柯、张峻凡
    描述：直尺的数据和行为
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using System.Linq;

[EntityResource(null, null, "Instruments/Ruler/Ruler")]
public class RulerInstrument : DirectMeasurementInstrumentBase
{
    public override int ID => 2;
    //override的变量和函数请参考基类InstrumentBase中的说明
    public override string InstName => "钢板尺"; 

    public override double URV { get; set; } = 300; 

    public override double LRV { get; set; } = 0; 

    public override double ErrorLimit { get; set; } = 0.5; 

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "毫米"; 

    public override string UnitSymbol  => "mm"; 

    public override string previewImagePath => "Instruments/Ruler/ruler_preview";

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }

    public override void ShowGameButton(List<GameButtonItem> buttonItems)
    {
        base.ShowGameButton(buttonItems);
        buttonItems.Where(x => x.GameObject.name.Equals("CloseButton")).FirstOrDefault().OnClick.Add(() =>
        {
            Entity.FindChildren("Ruler_son").GetComponent<Ruler_main>().UsingX();
        });
    }
}
