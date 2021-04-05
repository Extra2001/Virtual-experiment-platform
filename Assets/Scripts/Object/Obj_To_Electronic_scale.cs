using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_To_Electronic_scale : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public float t = 15.0f;
    public bool Doing = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name== "ElectronicScales")
        {
            Doing = false;
        }

    }

    private void absord()
    {
        GameObject ES = GameObject.Find("ElectronicScales");
        transform.position += (ES.transform.position - this.transform.position)/t;
    }
}
