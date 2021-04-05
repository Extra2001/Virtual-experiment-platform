using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/Ruler/rulerRescaled")]
public class RulerInstrument : InstrumentBase
{
    public override string InstName { get => "钢板尺"; }

    public override double URV { get => 500; }

    public override double LRV { get => 0; }

    public override double ErrorLimit { get => 0.5; }

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit { get => "毫米"; }

    public override string UnitSymbol { get => "mm"; }

    public override GameObject gameObject { get; set; }

    public override Sprite previewImage { get => Resources.Load<Sprite>("Instruments/Ruler/ruler_preview"); }

    public override double GetMeasureResult()
    {
        throw new System.NotImplementedException();
    }

    public override void InstReset()
    {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        Debug.Log("初始化");
    }

    /// <summary>
    /// 显示实体
    /// </summary>
    public override void OnShow()
    {
        base.OnShow();
    }

    /// <summary>
    /// 隐藏实体
    /// </summary>
    public override void OnHide()
    {
        base.OnHide();
    }

    /// <summary>
    /// 销毁实体
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// 实体逻辑刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    /// <summary>
    /// 重置实体
    /// </summary>
    public override void Reset()
    {
        base.Reset();
    }

}
