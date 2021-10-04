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
    public override int ID => 1;
    //仪器归类：直接测量仪器
    public override string InstName => "游标卡尺";
    //仪器名称字符
    public override double URV { get; set; } = 250;
    //仪器测量范围上界
    public override double LRV { get; set; } = 0;
    //仪器测量范围下界
    public override double ErrorLimit { get; set; } = 0.02;
    //仪器误差限
    public override double RandomErrorLimit { get; set; } 
    //仪器随机误差与零点误差
    public override double MainValue { get; set; }
    //仪器主值，包括误差
    public override string Unit => "毫米";
    //仪器单位字符
    public override string UnitSymbol => "mm";
    //仪器单位标识
    public override string previewImagePath => 
                            "Instruments/Caliper/caliper_preview";
    //仪器图片路径
    public override double GetMeasureResult() { 
        throw new System.NotImplementedException();}
        //仪器测量事件响应协议节点

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
