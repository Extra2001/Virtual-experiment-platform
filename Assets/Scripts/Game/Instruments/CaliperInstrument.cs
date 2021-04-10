using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/Caliper/Caliper")]
public class CaliperInstrument : InstrumentBase
{
    private Vector3 Position = new Vector3();
    private GameObject Self;

    public override string InstName { get => "游标卡尺"; }

    public override double URV { get => 500; }

    public override double LRV { get => 0; }

    public override double ErrorLimit { get => 0.5; }

    public override double RandomErrorLimit { get; set; }
    public override double MainValue { get; set; }

    public override string Unit { get => "毫米"; }

    public override string UnitSymbol { get => "mm"; }

    public override GameObject gameObject { get; set; }

    public override Sprite previewImage { get => Resources.Load<Sprite>("Instruments/Caliper/caliper_preview"); }

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
        Self = GameObject.Find("CaliperInstrument");
    }

    /// <summary>
    /// 显示实体
    /// </summary>
    public override void OnShow()
    {
        Position.x = RecordManager.tempRecord.InstrumentStartPosition[0]-12f;
        Position.y = RecordManager.tempRecord.InstrumentStartPosition[1]+4f;
        Position.z = RecordManager.tempRecord.InstrumentStartPosition[2]-5f;

        Self.transform.position = Position;
        base.OnShow();
    }
}
