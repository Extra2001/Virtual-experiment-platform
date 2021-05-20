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

    

    //�˴�״̬0�����ʼ��1�������ڹ������ʽ��2���������ʽ���
    public int AverageState { get; set; } = 0;
    public int UaState { get; set; } = 0;
    public int UbState { get; set; } = 0;
    public int ComplexState { get; set; } = 0;

}