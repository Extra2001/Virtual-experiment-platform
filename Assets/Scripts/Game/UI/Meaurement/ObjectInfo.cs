using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectInfo : HTBehaviour
{
    [SerializeField]
    private Button _Mask;
    [SerializeField]
    private GameObject _RootPanel;
    [SerializeField]
    private Text _Title;
    [SerializeField]
    private Text _Desc;
    [SerializeField]
    private InputField _ScaleInput;
    [SerializeField]
    private Slider _ScaleSlider;
    [SerializeField]
    private InputField _MassInput;
    [SerializeField]
    private Toggle _Gravity;

    private ObjectValue objectValue;

    private void Start()
    {
        _Mask.onClick.AddListener(() =>
        {
            Main.m_UI.CloseUI<ObjectInfoUILogic>();
        });
        _ScaleInput.onValueChanged.AddListener(x =>
        {
            objectValue.Scale = float.Parse(x);
            _ScaleSlider.value = float.Parse(x);
        });
        _ScaleSlider.onValueChanged.AddListener(x =>
        {
            objectValue.Scale = x;
            _ScaleInput.text = x.ToString();
        });
        _MassInput.onValueChanged.AddListener(x =>
        {
            objectValue.Mass = float.Parse(x);
        });
        _Gravity.onValueChanged.AddListener(x =>
        {
            objectValue.Gravity = x;
        });
    }

    public void Show(ObjectValue value)
    {
        objectValue = value;
        _Title.text = objectValue.ObjectModel.Name;
        _Desc.text = objectValue.ObjectModel.DetailMessage;
        _ScaleInput.text = objectValue.Scale.ToString();
        _ScaleSlider.value = objectValue.Scale;
        _MassInput.text = objectValue.Mass.ToString();
        _Gravity.isOn = objectValue.Gravity;

        _RootPanel.rectTransform().SetFloat();
    }
}
