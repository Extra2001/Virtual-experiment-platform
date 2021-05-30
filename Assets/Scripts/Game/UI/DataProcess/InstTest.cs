using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstTest : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public double Value;

    private double hh = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hh != Value)
        {
            hh = Value;
            GameManager.Instance.CurrentInstrument?.ShowValue(Value);
        }
    }
}
