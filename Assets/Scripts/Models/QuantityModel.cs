/************************************************************************************
    作者：荆煦添
    描述：用户选择的物理量信息
*************************************************************************************/
using System.Collections.Generic;
using System;

[Serializable]
public class QuantityModel
{
    public string Name { get; set; } = "";
    public string Symbol { get; set; } = "";
    public Type InstrumentType { get; set; } = typeof(RulerInstrument);
    public int Groups { get; set; } = 8;

    public List<string> Data { get; set; } = new List<string>();
    public DataColumnModel MesuredData { get; set; } = null;
    public DataColumnModel IndependentData { get; set; } = null;
    public DataColumnModel DifferencedData { get; set; } = null;

    //逐差法需要的内容
    public string stepLength { get; set; } = "1";

    //一元线性回归需要的内容
    public int nextValue = 0; // 0为b(斜率)，1为a
    public int dataset = 0; // 0为原始数据，1为自变量数据

    //存储输入的公式
    public List<FormulaNode> AverageExpression { get; set; } = null;
    public List<FormulaNode> UaExpression { get; set; } = null;
    public List<FormulaNode> UbExpression { get; set; } = null;
    public List<FormulaNode> ComplexExpression { get; set; } = null;
}
