using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;


    // Start is called before the first frame update

    void OnCollisionStay(Collision collision)
    {
        print("qwq");
        if ((collision.collider.tag == "Mesured"|| collision.collider.tag == "Anvil") && this.transform.parent.transform.Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num<=-0.5f)
        {
            this.transform.parent.transform.Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num += 0.5f;
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
