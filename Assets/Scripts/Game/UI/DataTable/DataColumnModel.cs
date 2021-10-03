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
    Mesured, // ԭʼ����
    Independent, // һԪ���Իع���Ա���
    Differenced, // ����ı��
    Graphic,     //ͼʾ���ı��
    SingleQuantity // ����������������
}
