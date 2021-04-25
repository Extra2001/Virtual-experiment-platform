using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thermometer : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    // Start is called before the first frame update
    public void Show_tem(double num)
    {
        this.transform.Find("Sphere").gameObject.GetComponent<thermometer_control>().ChangeTheTEM(num);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
