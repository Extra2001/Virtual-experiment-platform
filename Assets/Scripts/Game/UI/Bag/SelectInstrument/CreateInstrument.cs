using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateInstrument : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public Type InstrumentType;
    private Button btn;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CreateWithDestroy);
    }

    private static InstrumentBase Create(InstrumentInfoModel model)
    {
        var instrument = Main.m_Entity.GetEntity(model.instrumentType, model.instrumentType.Name) as InstrumentBase;
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
        return instrument;
    }

    private void CreateWithDestroy()
    {
        var inst = RecordManager.tempRecord.showedInstrument;
        if (inst != null && inst.instrumentType != null)
            Main.m_Entity.HideEntity(Main.m_Entity.GetEntity(inst.instrumentType, inst.instrumentType.Name));
        Create(new InstrumentInfoModel()
        {
            instrumentType = InstrumentType,
        });
        Main.m_UI.CloseUI<BagControl>();
    }

    public static void CreateRecord()
    {
        var inst = RecordManager.tempRecord.showedInstrument;
        if (inst != null && inst.instrumentType != null)
            Create(inst);
    }
}
