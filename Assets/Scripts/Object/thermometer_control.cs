using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thermometer_control : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public void ChangeTheTEM(double i)
    {
        double k = -0.00110375 * i + 0.0841875;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y,(float)k);
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
