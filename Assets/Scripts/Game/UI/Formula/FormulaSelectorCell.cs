using HT.Framework;
using Jint;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaSelectorCell : HTBehaviour
{
    public bool isBinaryExpression = false;

    public string value;

    private void Start()
    {
        JSEval("3*Math.PI");
    }

    public static double JSEval(string expression)
    {
        Engine engine = new Engine();
        double hh = engine.Execute(expression).GetCompletionValue().AsNumber();
        Debug.Log(hh);
        return hh;
    }
}
