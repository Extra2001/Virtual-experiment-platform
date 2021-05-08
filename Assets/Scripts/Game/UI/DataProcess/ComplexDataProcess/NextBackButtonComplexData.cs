using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextBackButtonComplexData : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public Button BackButton;
    public Button NextButton;

    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(Back);
        NextButton.onClick.AddListener(Next);
    }

    public void Back()
    {
        Main.m_Procedure.SwitchLastProcedure();
    }
    public void Next()
    {
        Main.m_Procedure.SwitchNextProcedure();
    }
}
