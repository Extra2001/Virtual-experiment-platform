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

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CreateOrDestory);
        ShowedInstrument = null;
    }

    private void CreateOrDestory()
    {
        //ShowedInstrument=
        if (ShowedInstrument != null)
        {

        }
        Main.m_Entity.CreateEntity(InstrumentType,loadDoneAction:x=>
        {

        });


        Main.m_UI.CloseUI<BagControl>();
    }
}
