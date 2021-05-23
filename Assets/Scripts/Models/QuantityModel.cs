using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class QuantityModel
{
    public string Name { get; set; } = "";
    public string Symbol { get; set; } = "";
    public Type InstrumentType { get; set; } = typeof(RulerInstrument);
    public int Groups { get; set; } = 8;

    public List<double> Data { get; set; } = new List<double>();

    public double Average { get; set; } = 0.0;
    public double Ua { get; set; } = 0.0;
    public double Ub { get; set; } = 0.0;
    public double Uncertain { get; set; } = 0.0;


    //此处状态0代表初始，1代表正在构建表达式，2代表构建表达式完成
    public int AverageState { get; set; } = 0;
    public int UaState { get; set; } = 0;
    public int UbState { get; set; } = 0;
    public int ComplexState { get; set; } = 0;

    //存储输入的公式
    public List<FormulaNode> AverageExpression { get; set; } = null;
    public List<FormulaNode> UaExpression { get; set; } = null;
    public List<FormulaNode> UbExpression { get; set; } = null;
    public List<FormulaNode> ComplexExpression { get; set; } = null;
}