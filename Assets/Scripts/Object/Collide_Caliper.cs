using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide_Caliper : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    void OnCollisionStay(Collision collision)
    {
        if ((collision.collider.tag == "Mesured") && this.transform.parent.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num <= -0.01f)
        {
            this.transform.parent.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num += 0.01f;
        }
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
