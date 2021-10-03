using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataColumnModel
{
    public string name { get; set; } = "";
    public bool addedToTable { get; set; } = false;
    public DataColumnType type { get; set; }
    public string quantitySymbol { get; set; }
    public List<string> data { get; set; } = new List<string>();
}

public enum DataColumnType
{
    Mesured, // 原始数据
    Independent, // 一元线性回归的自变量
    Differenced, // 逐差法后的表格
    Graphic,     //图示法的表格
    SingleQuantity // 单物理量的三个量
}
