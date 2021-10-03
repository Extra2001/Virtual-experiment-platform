/************************************************************************************
    作者：荆煦添
    描述：右键仪器信息的数据模型
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Reflection;
using System.Collections;
using DG.Tweening;

public class InstrumentInfo : HTBehaviour
{
    public double Step => Convert.ToDouble(_Step.text);

    [SerializeField]
    private Button _Mask;
    [SerializeField]
    private Text _Name;
    [SerializeField]
    private Text _LRV;
    [SerializeField]
    private Text _URV;
    [SerializeField]
    private Text _UnitSymbol;
    [SerializeField]
    private Text _Unit;
    [SerializeField]
    private InputField _MainValue;
    [SerializeField]
    private InputField _RandomError;
    [SerializeField]
    private Button _ConfirmButton;
    [SerializeField]
    private GameObject _RootPanel;
    [SerializeField]
    private Button _SwitchRange;
    [SerializeField]
    private InputField _Step;
    [SerializeField]
    private Button _StepAdd;
    [SerializeField]
    private Button _StepSub;

    private Dictionary<string, IntrumentInfoItem> infoItem = new Dictionary<string, IntrumentInfoItem>();
    private InstrumentBase _instrument;
    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!initialized)
            Initialize();
    }

    public void ShowInstrument(Type instrument)
    {
        if (!initialized)
            Initialize();
        foreach (var item in infoItem)
            item.Value.onValueChanged.Clear();
        _instrument = GameManager.Instance.GetInstrument(instrument);
        _instrument.ShowInfoPanel(infoItem);

        _RootPanel.SetFloatWithAnimation(this);
    }

    public void Hide()
    {
        _RootPanel.transform.DOScale(0, 0.3f)
            .SetEase(Ease.OutExpo);
        Invoke(nameof(Close), 0.3f);
    }

    public void Close()
    {
        Main.m_UI.CloseUI<InstrmentInfoUILogic>();
    }

    private void Initialize()
    {
        infoItem.Clear();
        foreach (var item in typeof(InstrumentInfo).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
        {
            UIBehaviour gameobj = null;
            try { gameobj = item.GetValue(this) as UIBehaviour; }
            catch { }
            if (gameobj != null)
            {
                infoItem.Add(item.Name, new IntrumentInfoItem()
                {
                    GameObject = gameobj.gameObject
                });
            }
        }
        _Mask.onClick.AddListener(() =>
        {
            foreach (var item in infoItem)
                item.Value.onValueChanged.Clear();
            Hide();
        });
        _MainValue.onValueChanged.AddListener(x =>
        {
            infoItem[nameof(_MainValue)].onValueChanged.ForEach(y => y.Invoke());
        });
        _RandomError.onValueChanged.AddListener(x =>
        {
            infoItem[nameof(_RandomError)].onValueChanged.ForEach(y => y.Invoke());
        });
        _ConfirmButton.onClick.AddListener(() =>
        {
            infoItem[nameof(_ConfirmButton)].onValueChanged.ForEach(y => y.Invoke());
            Hide();
        });
        _SwitchRange.onClick.AddListener(() =>
        {
            infoItem[nameof(_SwitchRange)].onValueChanged.ForEach(y => y.Invoke());
        });
        _StepAdd.onClick.AddListener(() =>
        {
            infoItem[nameof(_StepAdd)].onValueChangedDouble.ForEach(y => y.Invoke(Step));
        });
        _StepSub.onClick.AddListener(() =>
        {
            infoItem[nameof(_StepSub)].onValueChangedDouble.ForEach(y => y.Invoke(Step));
        });
        initialized = true;
    }
}


public class IntrumentInfoItem
{
    public GameObject GameObject { get; set; }
    public List<Action> onValueChanged { get; set; } = new List<Action>();
    public List<Action<string>> onValueChangedString { get; set; } = new List<Action<string>>();
    public List<Action<double>> onValueChangedDouble { get; set; } = new List<Action<double>>();
}
