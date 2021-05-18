using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_micrometer_father : HTBehaviour
{
    //启用自动化
    public int RotatePerTime;
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Find("Micrometer_grandson").Find("Obj3d66-1078630-1-677").GetComponent<Rotate_micrometer>().Rotatenum(RotatePerTime);
    }
}
