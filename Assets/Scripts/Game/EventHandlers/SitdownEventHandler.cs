using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
/// <summary>
/// 新建事件
/// </summary>
public class Sitdown : EventHandlerBase
{

    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public Sitdown Fill(float x, float y, float z)
    {
        Debug.Log("Sitdown");

        RecordManager.tempRecord.InstrumentStartPosition[0] = x;
        RecordManager.tempRecord.InstrumentStartPosition[1] = y;
        RecordManager.tempRecord.InstrumentStartPosition[2] = z;

        RecordManager.tempRecord.ObjectStartPosition[0] = x;
        RecordManager.tempRecord.ObjectStartPosition[1] = y + .2f;
        RecordManager.tempRecord.ObjectStartPosition[2] = z + 6f;

        return this;
    }

    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {

    }
}
