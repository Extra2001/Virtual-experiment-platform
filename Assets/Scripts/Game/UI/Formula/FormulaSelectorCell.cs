using HT.Framework;
using Jint;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class FormulaSelectorCell : HTBehaviour
{
    public VaribleExpression varibleExpression = 0;

    public string value;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            FormulaController.Instance.SelectCell(gameObject.name, value);
        });
    }
}

public enum VaribleExpression
{
    Zero,
    One,
    Two
}