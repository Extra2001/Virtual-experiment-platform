using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuredDifference2 : MeasuredUncertainty
{
    public override bool CheckAll()
    {
        if(base.CheckAll())
        {
            if (quantity.AverageExpression == null || quantity.AverageExpression.Count == 0)
            {
                ShowModel("�ϳɲ�ȷ���Ȼ�δ��д");
                return false;
            }
            return true;
        }
        return false;
    }
}
