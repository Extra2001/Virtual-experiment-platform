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
    private Type ShowedInstrument;
    private EntityLogicBase SelectedInstrument;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CreateOrDestory);
        ShowedInstrument = null;
    }

    private void CreateOrDestory()
    {
        ShowedInstrument = RecordManager.tempRecord.Instrumenttype;
        if (ShowedInstrument != null)
        {
            Main.m_Entity.HideEntity(Main.m_Entity.GetEntity(ShowedInstrument, ShowedInstrument.Name));
        }
        SelectedInstrument = Main.m_Entity.GetEntity(InstrumentType, InstrumentType.Name);
        Main.m_Entity.ShowEntity(SelectedInstrument);
        RecordManager.tempRecord.Instrumenttype = InstrumentType;

        Main.m_UI.CloseUI<BagControl>();
    }
}
