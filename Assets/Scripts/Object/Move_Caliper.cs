using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Caliper : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public float num;
    public void Movenum(float num)
    {
        this.transform.localPosition -= new Vector3(0.02f*num, 0, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movenum(num);

    }
}
