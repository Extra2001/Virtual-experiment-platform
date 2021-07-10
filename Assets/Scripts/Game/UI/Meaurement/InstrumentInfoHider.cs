/************************************************************************************
    作者：荆煦添
    描述：仪器信息自动隐藏器
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentInfoHider : HTBehaviour
{
    [ReorderableList]
    public List<GameObject> Operate = new List<GameObject>();
    [ReorderableList]
    public List<GameObject> Watch = new List<GameObject>();

    private void Update()
    {
        for(int i = 0; i < Watch.Count; i++)
            Operate[i].SetActive(Watch[i].activeSelf);
    }
}
