using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FormulaTest : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            foreach (var item in FormulaController.Instances)
            {
                try
                {
                    Log.Info($"{item.Value.Expression} = {item.Value.ExpressionExecuted}");
                }
                catch
                {
                    Log.Info("公式未输入完整。");
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
