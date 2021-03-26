using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeExp : HTBehaviour
{
    public int ExpId;

    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Main.m_Event.Throw(Main.m_ReferencePool.Spawn<ChooseExpEventHandler>().Fill(ExpId));
        });
    }
}
