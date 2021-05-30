using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmeterMatChanger : HTBehaviour
{
    [SerializeField]
    Material Normal;
    [SerializeField]
    Material Highlight;
    [SerializeField]
    int index;

    public void Normalize()
    {
        GetComponent<MeshRenderer>().material = Normal;
    }

    public void Highlighten()
    {
        GetComponent<MeshRenderer>().material = Highlight;
    }
}
