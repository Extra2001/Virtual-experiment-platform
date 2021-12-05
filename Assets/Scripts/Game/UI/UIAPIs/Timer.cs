using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : SingletonBehaviourBase<Timer>
{
    public string StartTime;
    public string TimeNow;

    private int i;
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        StartTime = DateTime.Now.ToString();
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //每100帧更新一次时间
        if (i == 100)
        {
            TimeNow = DateTime.Now.ToString();
            i = 0;
        }
        else
        {
            i++;
        }
    }


}
