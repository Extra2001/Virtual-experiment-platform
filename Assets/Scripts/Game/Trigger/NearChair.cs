using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearChair : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;


    private void OnTriggerStay(Collider other)
    {
        ShowTips();
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("dadadada");
            Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<Sitdown>().Fill());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CloseTips();
    }

    private void ShowTips()
    {
        //出现提示按E
    }
    private void CloseTips()
    {
        //消除提示按E
    }
}
