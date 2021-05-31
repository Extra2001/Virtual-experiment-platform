using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAmmeter : HTBehaviour
{
    //启用自动化
    public float MaxA = 3.0f;
    public float NowA=0.0f;
    private float TarA;
    private bool OnGoing = false;
    public float times = 15.0f;
    public float ii;
    protected override bool IsAutomate => true;

    public void ShowNum(float num)
    {
        transform.Find("Cylinder005").transform.DOLocalRotate(new Vector3(0, 207.5f-82.5f*num /MaxA, 0), 1f).SetEase(Ease.OutExpo);
    }




    private void Update()
    {

    }

}
