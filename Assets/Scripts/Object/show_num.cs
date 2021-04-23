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


    // Start is called before the first frame update
    void Start()
    {
         f1 = this.transform.Find("1").gameObject;
         f2 = this.transform.Find("2").gameObject;
         f3 = this.transform.Find("3").gameObject;
         f4 = this.transform.Find("4").gameObject;
         f5 = this.transform.Find("5").gameObject;
         f6 = this.transform.Find("6").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        f1.layer = 12;
    }
}
