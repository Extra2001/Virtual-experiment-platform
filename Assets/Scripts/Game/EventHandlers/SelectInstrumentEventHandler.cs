using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
/// <summary>
/// 新建事件
/// </summary>
public class SelectInstrumentEventHandler : EventHandlerBase
{
    public InstrumentBase ActiveInstrument;
    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public SelectInstrumentEventHandler Fill(InstrumentBase instrument)//获取当前处于活跃状态的仪器
    {
        ActiveInstrument = instrument;
        return this;
    }

    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {
        
    }
}
