using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VariablePanel : HTBehaviour
{
    [SerializeField]
    private InputField _Symbol;
    [SerializeField]
    private InputField _Value;
    [SerializeField]
    private InputField _Uncertainty;
    [SerializeField]
    private Button _Insert;
    [SerializeField]
    private Button _Delete;

    [NonSerialized]
    public ValidNumberPanel Panel;
    [NonSerialized]
    public ValueNumberVariableModel Model;

    // Start is called before the first frame update
    void Start()
    {
        _Symbol.onValueChanged.AddListener(x =>
        {
            Model.Symbol = x;
        });
        _Value.onValueChanged.AddListener(x =>
        {
            Model.Value = x;
        });
        _Uncertainty.onValueChanged.AddListener(x =>
        {
            Model.Uncertainty = x;
        });
        _Insert.onClick.AddListener(() =>
        {
            Panel.AddExpr(_Symbol.text);
        });
        _Delete.onClick.AddListener(() =>
        {
            Panel.DeleteVariable(gameObject);
        });
    }
}
