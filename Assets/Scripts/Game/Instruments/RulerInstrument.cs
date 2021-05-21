using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/Ruler/Ruler")]
public class RulerInstrument : InstrumentBase
{
    private Vector3 Position = new Vector3();
    private GameObject Self;

    public override string InstName => "钢板尺"; 

    public override double URV  => 500; 

    public override double LRV => 0; 

    public override double ErrorLimit  => 0.5; 

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit => "毫米"; 

    public override string UnitSymbol  => "mm"; 

    public override string previewImagePath => "Instruments/Ruler/ruler_preview";

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
        Self = GameObject.Find("RulerInstrument");
    }

    /// <summary>
    /// 显示实体
    /// </summary>
    public override void OnShow()
    {
        Position.x = RecordManager.tempRecord.InstrumentStartPosition[0]+2f;
        Position.y = RecordManager.tempRecord.InstrumentStartPosition[1]-4.5f;
        Position.z = RecordManager.tempRecord.InstrumentStartPosition[2]-18f;

        Self.transform.position = Position;
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

    public override void ShowValue(double value)
    {
        throw new System.NotImplementedException();
    }
}
