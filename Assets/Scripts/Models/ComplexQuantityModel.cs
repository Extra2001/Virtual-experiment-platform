using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ComplexQuantityModel
{
    //用于存储最终合成量的信息
    public string AnswerAverage { get; set; }
    public string AnswerUncertain { get; set; }

    public List<FormulaNode> AverageExpression { get; set; } = null;
    public List<FormulaNode> UncertainExpression { get; set; } = null;
    
}
