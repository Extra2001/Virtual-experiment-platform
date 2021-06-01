using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thermometer_main : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    // Start is called before the first frame update、
    public void ShowNum(float num)
    {
        transform.Find("Cylinder001").transform.DOScaleZ(0.67f+num*1.1f/50f, 1f).SetEase(Ease.OutExpo);
    }


    void Start()
    {
        ShowNum(23);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
