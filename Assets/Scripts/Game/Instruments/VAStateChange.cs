using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VAStateChange : HTBehaviour
{

    [SerializeField]
    Material RedNormal;
    [SerializeField]
    Material RedHighlight;
    [SerializeField]
    Material BlackNormal;
    [SerializeField]
    Material BlackHighlight;
    [SerializeField]
    GameObject[] Cylinder = new GameObject[3];

    public void SwitchState1()//0-3V或0-0.6A
    {
        Cylinder[0].GetComponent<MeshRenderer>().material = BlackHighlight;
        Cylinder[1].GetComponent<MeshRenderer>().material = RedHighlight;
        Cylinder[2].GetComponent<MeshRenderer>().material = RedNormal;
    }

    public void SwitchState2()
    {
        Cylinder[0].GetComponent<MeshRenderer>().material = BlackHighlight;
        Cylinder[1].GetComponent<MeshRenderer>().material = RedNormal;
        Cylinder[2].GetComponent<MeshRenderer>().material = RedHighlight;
    }
}
