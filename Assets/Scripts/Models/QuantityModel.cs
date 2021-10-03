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

    //物理量的数据处理方法，0为未处理，1为直接，2为逐差法，3为一元线性回归
    public int processMethod { get; set; } = 1;

    public DataColumnModel MesuredData { get; set; } = null; //测量原始数据
    public DataColumnModel IndependentData { get; set; } = null; //一元线性回归和图示法的自变量
    public DataColumnModel DifferencedData { get; set; } = null; //逐差法逐差后数据

    //逐差法需要的内容
    public string stepLength { get; set; } = "1";

    //一元线性回归需要的内容
    public int nextValue = 0; // 0为b(斜率)，1为a
    public int dataset = 0; // 0为原始数据，1为自变量数据
    public List<FormulaNode> BExpression { get; set; } = null;
    public List<FormulaNode> AExpression { get; set; } = null;
    public List<FormulaNode> RelationExpression { get; set; } = null;

    //存储输入的公式
    public List<FormulaNode> AverageExpression { get; set; } = null;
    public List<FormulaNode> UaExpression { get; set; } = null;
    public List<FormulaNode> UbExpression { get; set; } = null;
    public List<FormulaNode> ComplexExpression { get; set; } = null;
}
