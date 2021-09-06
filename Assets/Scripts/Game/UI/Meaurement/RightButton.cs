/************************************************************************************
    作者：荆煦添
    描述：右键单击仪器处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using System;

public class RightButton : HTBehaviour
{
    public Type InstrumentType;

    private void Update()
    {
        if (Main.m_Procedure.CurrentProcedure is OnChair && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (var item in gameObject.transform.GetComponentsInChildren<Collider>(true))
                    if (hit.collider.gameObject.GetInstanceID().Equals(item.gameObject.GetInstanceID()))
                        Main.m_UI.OpenTemporaryUI<InstrmentInfoUILogic>(InstrumentType);
            }
        }
    }
}