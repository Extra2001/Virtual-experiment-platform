using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormulaTest : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Log.Info(FormulaController.Instance.Expression);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
