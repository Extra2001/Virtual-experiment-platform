using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NextButton : HTBehaviour
{
    public HHHH nextProcedure;

    public Type[] types = { typeof(AddValueEventHandler), typeof(EnterExpressionEventHandler), typeof(PreviewConfirmEventHandler) };

    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Main.m_Event.Throw(types[(int)nextProcedure]);
        });
    }

    public enum HHHH
    {
        EnterExpression,
        Preview,
        EnterClassroom
    }
}
