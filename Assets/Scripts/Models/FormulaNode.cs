/************************************************************************************
    作者：荆煦添
    描述：公式编辑器的节点信息
*************************************************************************************/
using System.Collections.Generic;
using System;

[Serializable]
public class FormulaNode
{
    public string PrefabName { get; set; }
    public string GUID { get; set; }
    public string value { get; set; }
    public string name { get; set; }
    public List<string> ReplaceFlags { get; set; } = new List<string>();
}
