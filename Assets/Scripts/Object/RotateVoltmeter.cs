using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateVoltmeter : HTBehaviour
{
    //启用自动化
    public float MaxV=3.0f;
    private float PreV=0.0f;
    private float NowV;
    private float TarV;
    private bool OnGoing = false;
    public float times = 15.0f;
    protected override bool IsAutomate => true;

    public void ShowNum(float num)
    {
        transform.Find("Cylinder004").transform.DOLocalRotate(new Vector3(0, 207.5f - 82.5f * num / MaxV, 0), 1f).SetEase(Ease.OutExpo);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
      
    }
}
