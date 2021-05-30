using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAmmeter : HTBehaviour
{
    //启用自动化
    public float MaxA = 3.0f;
    private float NowA=0.0f;
    private float TarA;
    private bool OnGoing = false;
    public float times = 15.0f;
    protected override bool IsAutomate => true;

    private void ChangeTheRotate(float num)
    {
        transform.Find("Cylinder005").transform.DOLocalRotate(new Vector3(0, num * 110.0f * 5.0f / 6.0f / (MaxA * 4.0f / 3.0f), 0), .3f)
            .SetEase(Ease.OutExpo);
    }


    public void ShowNum(float num)
    {

        ChangeTheRotate((num - NowA));
        NowA = num;
    }

    private void Update()
    {
        ChangeTheRotate(0.1f);
    }

}
