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



    

    public int AverageState { get; set; } = 0;
    public int UncertainState { get; set; } = 0;
    public int AnswerAverageState { get; set; } = 0;
    public int AnswerUncertainState { get; set; } = 0;

    public double Average { get; set; } = 0.0;
    public double Uncertain { get; set; } = 0.0;
    public double AnswerAverage { get; set; } = 0.0;
    public double AnswerUncertain { get; set; } = 0.0;

    public List<FormulaNode> AverageExpression { get; set; } = null;
    public List<FormulaNode> UncertainExpression { get; set; } = null;
    public List<FormulaNode> AnswerAverageExpression { get; set; } = null;
    public List<FormulaNode> AnswerUncertainExpression { get; set; } = null;
}