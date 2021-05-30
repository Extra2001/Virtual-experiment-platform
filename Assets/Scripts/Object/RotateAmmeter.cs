using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAmmeter : HTBehaviour
{
    //启用自动化
    public float MaxA = 0.6f;
    private float PreA = 0.0f;
    private float NowA;
    private float TarA;
    private bool OnGoing = false;
    public float times = 15.0f;
    protected override bool IsAutomate => true;

    private void ChangeTheRotate(float num)
    {
        transform.Find("Cylinder005").transform.DORotate(new Vector3(0, -num * 110.0f * 5.0f / 6.0f / (MaxA * 4.0f / 3.0f), 0), .3f)
            .SetEase(Ease.OutExpo);
    }


    public void ShowNum(float num)
    {
        TarA = num;
        OnGoing = true;
        NowA = PreA;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    ShowNum(0.2f);
        //}
        //测试样例
        
        if (OnGoing)
        {
            ChangeTheRotate((TarA - NowA) / times);
            NowA = NowA + (TarA - NowA) / times;
            if ((TarA - NowA) <= 0.001 && (TarA - NowA) >= -0.001)
            {
                ChangeTheRotate((TarA - NowA));
                NowA = TarA;
                PreA = TarA;
                OnGoing = false;
            }
        }
    }
}
