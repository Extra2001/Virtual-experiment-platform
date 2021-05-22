using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InstrumentInfo : HTBehaviour
{
    public Button _Mask;
    public Text _Name;
    public Text _LRV;
    public Text _URV;
    public Text _UnitSymbol;
    public Text _Unit;
    public InputField _MainValue;
    public InputField _RandomError;
    public Button _ConfirmButton;
    public GameObject _RootPanel;

    private InstrumentBase _instrument;

    // Start is called before the first frame update
    void Start()
    {
        _Mask.onClick.AddListener(() =>
        {
            Main.m_UI.CloseUI<InstrmentInfoUILogic>();
        });
        _ConfirmButton.onClick.AddListener(() =>
        {
            double re = Convert.ToDouble(_RandomError.text);
            double mainValue = Convert.ToDouble(_MainValue.text);
            if (re > _instrument.ErrorLimit)
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString("随机误差不能大于仪器误差限")
                });
            else if (mainValue > _instrument.URV || mainValue < _instrument.LRV)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString("主值不能超过量程")
                });
            }
            else
            {
                Main.m_UI.CloseUI<UILogicTemporary>();
                _instrument.RandomErrorLimit = re;
                _instrument.MainValue = mainValue;
                _instrument.ShowValue(mainValue);
            }
        });
    }

    public void ShowInstrument(Type instrument)
    {
        var instance = instrument.CreateInstrumentInstance();
        _instrument = instance;

        _Name.text = instance.InstName;
        _LRV.text = instance.LRV.ToString();
        _URV.text = instance.URV.ToString();
        _Unit.text = instance.Unit;
        _UnitSymbol.text = instance.UnitSymbol;
        _MainValue.text = instance.MainValue.ToString();
        _RandomError.text = instance.RandomError.ToString();

        _RootPanel.rectTransform().SetFloat();
    }
}
