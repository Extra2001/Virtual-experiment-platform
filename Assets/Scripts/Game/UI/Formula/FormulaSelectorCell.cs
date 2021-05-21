using HT.Framework;
using Jint;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class FormulaSelectorCell : HTBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum ValueType
    {
        Normal,
        Measured,
        Complex
    }

    public FormulaController FormulaControllerInstance;
    [SerializeField]
    private string Title;
    [SerializeField]
    private string Desc;
    [Space]
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

    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke();
        FormulaControllerInstance.Indicator.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Invoke("ShowIndicator", 0.8f);
    }

    private void ShowIndicator()
    {
        print($"Õ£¡Ù‘⁄{gameObject.name}");
        FormulaControllerInstance.Indicator.ShowIndicate(Title, Desc);
    }
}