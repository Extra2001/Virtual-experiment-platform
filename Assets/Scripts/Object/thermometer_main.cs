using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

public class thermometer_main : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public bool IsC=true;

    // Start is called before the first frame update、
    public void ShowNum(float num)
    {

        if (IsC)
        {
            transform.Find("Cylinder001").transform.DOScaleZ(0.67f + num * 1.1f / 50f, 1f).SetEase(Ease.OutExpo);
        }
        else
        {
            transform.Find("Cylinder001").transform.DOScaleZ(0.67f + (32f+num*1.8f) * 1.1f / 50f, 1f).SetEase(Ease.OutExpo);
        }
    
    }


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
