using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuredDifference2 : MeasuredUncertainty
{
    public override bool CheckAll(bool silent = false)
    {
        if (quantity.AverageExpression == null || quantity.AverageExpression.Count == 0)
        {
            if (!silent)
                ShowModel("ƽ��ֵ��δ��д");
            return false;
        }
        if (base.CheckAll(silent))
        {            
            return true;
        }
        return false;
    }
}
