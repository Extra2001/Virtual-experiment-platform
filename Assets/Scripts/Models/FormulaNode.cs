using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
