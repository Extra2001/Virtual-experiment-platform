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
    public Type InstrumentType;
    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CreateWithDestroy);
    }
    /// <summary>
    /// 生成测量仪器
    /// </summary>
    private static InstrumentBase Create(InstrumentInfoModel model)
    {
        var instrument = GameManager.Instance.GetInstrument(model);
        var recpos = RecordManager.tempRecord.instrumentStartPosition;
        var position = new Vector3();
        position.x = recpos[0];
        position.y = recpos[1];
        position.z = recpos[2];
        instrument.Entity.transform.position = position;
        if (model.Valid)
        {
            instrument.Entity.transform.GetChild(0).position = model.position;
            instrument.Entity.transform.GetChild(0).rotation = model.rotation;
            instrument.MainValue = model.MainValue;
            instrument.RandomErrorLimit = model.RandomErrorLimit;
        }
        Main.m_Entity.ShowEntity(instrument);
        RecordManager.tempRecord.showedInstrument = model;
        Main.m_Event.Throw(Main.m_ReferencePool.Spawn<SelectInstrumentEventHandler>().Fill(instrument));
        return instrument;
    }
    /// <summary>
    /// 生成前销毁旧仪器
    /// </summary>
    private void CreateWithDestroy()
    {
        var inst = RecordManager.tempRecord.showedInstrument;
        if (inst != null && inst.instrumentType != null)
            Main.m_Entity.HideEntity(GameManager.Instance.GetInstrument(inst));
        Create(new InstrumentInfoModel()
        {
            instrumentType = InstrumentType,
        });
        Main.m_UI.CloseUI<BagControl>();
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
