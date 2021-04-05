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
    private bool IfConsistent;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CreateOrDestory);
        IfConsistent = false;
    }

    private void CreateOrDestory()
    {
        if (IfConsistent)
        {

        }
        else
        {

        }
        Main.m_UI.CloseUI<BagControl>();
    }
}
