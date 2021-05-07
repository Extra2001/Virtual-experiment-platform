using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show_num : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    GameObject f1;
    GameObject f2;
    GameObject f3;
    GameObject f4;
    GameObject f5;
    GameObject f6;
    GameObject f7;

    // Start is called before the first frame update
    void Start()
    {
         f1 = this.transform.Find("1").gameObject;
         f2 = this.transform.Find("2").gameObject;
         f3 = this.transform.Find("3").gameObject;
         f4 = this.transform.Find("4").gameObject;
         f5 = this.transform.Find("5").gameObject;
         f6 = this.transform.Find("6").gameObject;
         f7 = this.transform.Find("7").gameObject;
        f1.layer = 12;
        f2.layer = 12;
        f3.layer = 12;
        f4.layer = 12;
        f5.layer = 12;
        f6.layer = 12;
        f7.layer = 12;
    }
    public void ChangeTheNum(int i)
    {
        if (i == 0)
        {
            f1.layer = 0;
            f2.layer = 0;
            f3.layer = 0;
            f4.layer = 0;
            f5.layer = 0;
            f6.layer = 0;
            f7.layer = 12;
        }
        if (i == 1)
        {
            f1.layer = 12;
            f2.layer = 12;
            f3.layer = 0;
            f4.layer = 12;
            f5.layer = 0;
            f6.layer = 12;
            f7.layer = 12;
        }
        if (i == 2)
        {
            f1.layer = 0;
            f2.layer = 12;
            f3.layer = 0;
            f4.layer = 0;
            f5.layer = 12;
            f6.layer = 0;
            f7.layer = 0;
        }
        if (i == 3)
        {
            f1.layer = 0;
            f2.layer = 12;
            f3.layer = 0;
            f4.layer = 12;
            f5.layer = 0;
            f6.layer = 0;
            f7.layer = 0;
        }
        if (i == 4)
        {
            f1.layer = 12;
            f2.layer = 0;
            f3.layer = 0;
            f4.layer = 12;
            f5.layer = 0;
            f6.layer = 12;
            f7.layer = 0;
        }
        if (i == 5)
        {
            f1.layer = 0;
            f2.layer = 0;
            f3.layer = 12;
            f4.layer = 12;
            f5.layer = 0;
            f6.layer = 0;
            f7.layer = 0;
        }
        if (i == 6)
        {
            f1.layer = 0;
            f2.layer = 0;
            f3.layer = 12;
            f4.layer = 0;
            f5.layer = 0;
            f6.layer = 0;
            f7.layer = 0;
        }
        if (i == 7)
        {
            f1.layer = 0;
            f2.layer = 12;
            f3.layer = 0;
            f4.layer = 12;
            f5.layer = 0;
            f6.layer = 12;
            f7.layer = 12;
        }
        if (i == 8)
        {
            f1.layer = 0;
            f2.layer = 0;
            f3.layer = 0;
            f4.layer = 0;
            f5.layer = 0;
            f6.layer = 0;
            f7.layer = 0;
        }
        if (i == 9)
        {
            f1.layer = 0;
            f2.layer = 0;
            f3.layer = 0;
            f4.layer = 12;
            f5.layer = 0;
            f6.layer = 0;
            f7.layer = 0;
        }

    }
    // Update is called once per frame
    void Update()
    {
    }
}
