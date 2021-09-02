using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ResultScore
{
    public int MeasureQuantityError { get; set; } = 0;
    public int ComplexQuantityError { get; set; } = 0;
    public int DataRecordError { get; set; } = 0;

    public double CalcScore()
    {
        double ans = 1;
        ans += f(MeasureQuantityError) + f(ComplexQuantityError) + f(DataRecordError);

        return ans;
    }
    
    double f(int n)   // 将错误个数得到的分数归一化
    {
        double ans;
        if (n == 0)
        {
            ans = 1;
        }else if (n <= 2)
        {
            ans = 1 - 0.15 * n;
        }else if (n <= 5)
        {
            ans = 1 - 0.12 * n;
        }
        else
        {
            ans = 0;
        }
        return ans;
    }


}
