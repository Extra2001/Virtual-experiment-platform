using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EntityResource(null, null, "Instruments/Micrometer/Micrometer")]

public class MicrometerInstrument : InstrumentBase
{
    private Vector3 Position = new Vector3();
    private GameObject Self;

    public override string InstName => "螺旋测微器";

    public override double URV  => 500; 

    public override double LRV  => 0; 

    public override double ErrorLimit  => 0.5; 

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

    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        Self = GameObject.Find("MicrometerInstrument");
    }

    /// <summary>
    /// 显示实体
    /// </summary>
    public override void OnShow()
    {
        Position.x = RecordManager.tempRecord.InstrumentStartPosition[0]+27f;
        Position.y = RecordManager.tempRecord.InstrumentStartPosition[1]-15f;
        Position.z = RecordManager.tempRecord.InstrumentStartPosition[2]+8f;

        Self.transform.position = Position;
        base.OnShow();
    }

    public override void ShowValue(double value)
    {
        //Self.transform.Find
        throw new System.NotImplementedException();
    }
}
