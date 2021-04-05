using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        GameObject ES = GameObject.Find("ElectronicScales");
                if (Vector3.Distance(ES.transform.position, this.transform.position) <= 3.0f && Doing)
                    {
                        absord();
                    }
                    else if (Vector3.Distance(ES.transform.position,this.transform.position) > 3.0f)
                    {
                        Doing = true;
                    }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag== "Tools_Be_Moved")
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
