using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Micrometer_main : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public float ZeroPointError = 0.0f;

    private bool IsKaKaed=false;//别骂了别骂了，这个名字搞笑的。


    public void Measure()
    {
        if (IsKaKaed)
        {
            //播放螺旋测微计kaka的声音
        }
        else
        {
            IsKaKaed = true;
        }
        this.transform.Find("Micrometer_grandson").Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num = -3;
    }

    private void Look_back()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.Find("Micrometer_grandson").Find("rotatebody_main").Find("srick").transform.localPosition -= new Vector3(0, (0.53f * ZeroPointError) / 5000f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
