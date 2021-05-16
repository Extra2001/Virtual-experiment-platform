using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_micrometer : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public int num;
    private int prenum;
    public void NumShow(int num)
    {
        this.transform.Rotate(new Vector3(0, num*50/360, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (num != prenum)
        {
            NumShow(num-prenum);
            prenum = num;
        }
    }
}
