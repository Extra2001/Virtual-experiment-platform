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
    public double UA { get; set; } = 0.0;
    public double UB { get; set; } = 0.0;

    
    public int AverageState { get; set; } = (int)InputState.Start;
    public int UaState { get; set; } = (int)InputState.Start;
    public int UbState { get; set; } = (int)InputState.Start;
    public int ComplexState { get; set; } = (int)InputState.Start;

    public enum InputState
    {
        Start=0,
        Working=1,
        End=2
    }
}