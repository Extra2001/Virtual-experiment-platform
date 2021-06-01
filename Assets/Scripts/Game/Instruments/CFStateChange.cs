using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFStateChange : HTBehaviour
{

    [SerializeField]
    Material RedNormal;
    [SerializeField]
    Material RedHighlight;
    [SerializeField]
    GameObject[] Cylinder = new GameObject[2];

    public void SwitchState1()
    {
        Cylinder[0].GetComponent<MeshRenderer>().material = RedHighlight;
        Cylinder[1].GetComponent<MeshRenderer>().material = RedNormal;
    }

    public void SwitchState2()
    {
        Cylinder[0].GetComponent<MeshRenderer>().material = RedNormal;
        Cylinder[1].GetComponent<MeshRenderer>().material = RedHighlight;
    }
}
