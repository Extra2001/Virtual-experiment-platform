using HT.Framework;
using Jint;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormulaSelectorCell : HTBehaviour
{
    public VaribleExpression varibleExpression = 0;

    public List<string> ReplaceFlags = new() { "{0}", "{1}" };

    public string value;

    private void Start()
    {
        JSEval("3*Math.PI");
    }

    public void GenerateReplaceFlags()
    {
        var str0 = Guid.NewGuid().ToString("N");
        var str1 = Guid.NewGuid().ToString("N");
        value = value.Replace(ReplaceFlags[0], str0).Replace(ReplaceFlags[1], str1);
        ReplaceFlags[0] = str0;
        ReplaceFlags[1] = str1;
    }

    public static double JSEval(string expression)
    {
        Engine engine = new Engine();

        double hh = engine.Execute(expression).GetCompletionValue().AsNumber();
        Debug.Log(hh);
        return hh;
    }
}

public enum VaribleExpression
{
    Zero,
    One,
    Two
}