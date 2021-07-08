using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_micrometer : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public float num;
    private int prenum;
    public void Rotatenum(float num)
    {
        num = num / 5.0f;

            this.transform.localPosition -= new Vector3(0, (0.53f * num) / 5000f, 0);

            //this.transform.parent.Find("对象014").Find("Camera").localPosition -= new Vector3(0, (0.53f*num)/5000f, 0);
            //神秘bug

            this.transform.Rotate(new Vector3(0, num / 50.0f * 360.0f, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotatenum(num);
    }
}
