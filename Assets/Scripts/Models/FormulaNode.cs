/************************************************************************************
    ���ߣ�������
    ��������ʽ�༭���Ľڵ���Ϣ
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
