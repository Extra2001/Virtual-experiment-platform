using HT.Framework;
using Jint;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;

public class FormulaSelectorCell : HTBehaviour
{
    public enum ValueType
    {
        Normal,
        Measured,
        Complex
    }

    public FormulaController FormulaControllerInstance;
    [SerializeField]
    private Text Text;
    [Space]
    [SerializeField]
    private ValueType valueType;
    [SerializeField]
    private MeasuredStatisticValue MeasuredStatisticValue;
    [SerializeField]
    private ComplexStatisticValue ComplexStatisticValue;
    [Space]
    public string value;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (valueType == ValueType.Measured)
                value = (Main.m_Procedure.CurrentProcedure as MeasuredDataProcessProcedure)?.GetStatisticValue(MeasuredStatisticValue);
            else if (valueType == ValueType.Complex)
                value = (Main.m_Procedure.CurrentProcedure as ComplexDataProcessProcedure)?.GetStatisticValue(Text.text, ComplexStatisticValue);
            FormulaControllerInstance.SelectCell(gameObject.name, value);
        });
    }

    public void SetSelectorName(string name)
    {
        if (Text)
            Text.text = name;
    }
}