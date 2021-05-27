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

    private void ChangeTheRotate(float num)
    {
        this.transform.Find("Cylinder004").transform.Rotate(new Vector3(0,num* 110.0f*2.0f/3.0f / (MaxV*4.0f/3.0f),0));
    }


    public void ShowNum(float num)
    {
        TarV = num;
        OnGoing = true;
        NowV = PreV;
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
        //    ShowNum(3);
        //}
        //测试样例

            if (OnGoing)
        {
            ChangeTheRotate((TarV - NowV) / times);
            NowV=NowV+(TarV-NowV)/ times;
            if ((TarV - NowV) <= 0.01 && (TarV - NowV) >= -0.01)
            {
                ChangeTheRotate((TarV - NowV));
                NowV = TarV;
                PreV = TarV;
                OnGoing = false;
            }
        }
    }
}
