/************************************************************************************
    作者：张峻凡
    描述：生成被测物体
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateInstrument : HTBehaviour
{
    /// <summary>
    /// 生成测量仪器（初始化）
    /// </summary>
    public static InstrumentBase Create(Type instrumentType)
    {
        var instrument = GameManager.Instance.GetInstrument(instrumentType);
        var recpos = RecordManager.tempRecord.instrumentStartPosition;
        var position = new Vector3();
        position.x = recpos[0];
        position.y = recpos[1];
        position.z = recpos[2];
        instrument.Entity.transform.position = position;
        Main.m_Entity.ShowEntity(instrument);
        RecordManager.tempRecord.showedInstrument = new InstrumentInfoModel()
        {
            instrumentType = instrumentType
        };
        Main.m_Event.Throw(Main.m_ReferencePool.Spawn<SelectInstrumentEventHandler>().Fill(instrument));
        return instrument;
    }
    /// <summary>
    /// 生成测量仪器（带记录数据）
    /// </summary>
    public static InstrumentBase Create(InstrumentInfoModel model)
    {
        var instrument = GameManager.Instance.GetInstrument(model);
        var recpos = RecordManager.tempRecord.instrumentStartPosition;
        var position = new Vector3();
        position.x = recpos[0];
        position.y = recpos[1];
        position.z = recpos[2];
        // 恢复记录
        instrument.Entity.transform.GetChild(0).position = model.position;
        instrument.Entity.transform.GetChild(0).rotation = model.rotation;
        instrument.MainValue = model.MainValue;
        instrument.RandomErrorLimit = model.RandomErrorLimit;
        // 显示仪器
        Main.m_Entity.ShowEntity(instrument);
        RecordManager.tempRecord.showedInstrument = model;
        Main.m_Event.Throw(Main.m_ReferencePool.Spawn<SelectInstrumentEventHandler>().Fill(instrument));
        return instrument;
    }
    /// <summary>
    /// 隐藏现有仪器
    /// </summary>
    public static void HideCurrent()
    {
        var inst = RecordManager.tempRecord.showedInstrument;
        if (inst != null && inst.instrumentType != null)
            Main.m_Entity.HideEntity(GameManager.Instance.GetInstrument(inst));
    }
    /// <summary>
    /// 生成存档中的仪器
    /// </summary>
    public static void CreateRecord()
    {
        var inst = RecordManager.tempRecord.showedInstrument;
        if (inst != null && inst.instrumentType != null)
            Create(inst);
    }
}
