using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleModelPanel : HTBehaviour
{
    public Text _Title;
    public Text _Content;
    public GameObject _With;
    public GameObject _Without;
    public Button _Confirm1;
    public Button _Confirm2;
    public Button _Cancel;
    public Button _Mask;
    public GameObject _RootPanel;

    private SimpleModel simpleModel = null;

    void Start()
    {
        _Mask.onClick.AddListener(Hide);
        _Confirm1.onClick.AddListener(() =>
        {
            Hide();
            simpleModel.ConfirmAction?.Invoke();
        });
        _Confirm2.onClick.AddListener(() =>
        {
            Hide();
            simpleModel.ConfirmAction?.Invoke();
        });
        _Cancel.onClick.AddListener(() =>
        {
            Hide();
            simpleModel.CancelAction?.Invoke();
        });
    }

    public void Hide()
    {
        _RootPanel.transform.DOScale(0, 0.3f)
            .SetEase(Ease.OutExpo);
        Invoke(nameof(Close), 0.3f);
    }

    private void Close()
    {
        Main.m_UI.GetOpenedUI<SimpleModelPanelUILogic>().NavigateBack();
    }

    public void ShowModel(SimpleModel model)
    {
        _RootPanel.SetFloatWithAnimation(this);
        simpleModel = model;
        _Title.text = model.Title;
        _Content.text = model.Message;
        _Confirm1.GetComponentInChildren<Text>(true).text = model.ConfirmText;
        _Confirm2.GetComponentInChildren<Text>(true).text = model.ConfirmText;
        _Cancel.GetComponentInChildren<Text>(true).text = model.CancelText;
        if (model.ShowCancel)
        {
            _Without.SetActive(false);
            _With.SetActive(true);
        }
        else
        {
            _Without.SetActive(true);
            _With.SetActive(false);
        }
    }
}
