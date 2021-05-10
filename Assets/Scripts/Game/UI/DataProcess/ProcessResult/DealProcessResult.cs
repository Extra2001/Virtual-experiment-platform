using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealProcessResult : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public GameObject PerfectTips;
    public GameObject MistakeTips;
    public GameObject WrongFormula;
    public GameObject RightFormula;

    // Start is called before the first frame update
    void Start()
    {
        if (false)
        {
            //如果前无错
            PerfectTips.SetActive(true);
            MistakeTips.SetActive(false);
        }
        else
        {
            PerfectTips.SetActive(false);
            MistakeTips.SetActive(true);
        }
    
    }


}
