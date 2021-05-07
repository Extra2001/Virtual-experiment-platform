using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class ElectronicScalesSource_changenum : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public double qwq;
    public int accuracy_ratings;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Find("num").gameObject.GetComponent<manager_num>().accuracy_rating = accuracy_ratings;
        this.transform.Find("num").gameObject.GetComponent<manager_num>().Show_num(qwq);
    }
}
