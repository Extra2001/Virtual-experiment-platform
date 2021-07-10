/************************************************************************************
    作者：张峻凡
    描述：电压表电流表不同量程的展示动画
*************************************************************************************/
using HT.Framework;
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

    public void SwitchState1()//-1--3V或-0.2--0.6A
    {
        Cylinder[0].GetComponent<MeshRenderer>().material = BlackHighlight;
        Cylinder[1].GetComponent<MeshRenderer>().material = RedHighlight;
        Cylinder[2].GetComponent<MeshRenderer>().material = RedNormal;
    }

    public void SwitchState2()//-5--15V或-1--3A
    {
        Cylinder[0].GetComponent<MeshRenderer>().material = BlackHighlight;
        Cylinder[1].GetComponent<MeshRenderer>().material = RedNormal;
        Cylinder[2].GetComponent<MeshRenderer>().material = RedHighlight;
    }
}
