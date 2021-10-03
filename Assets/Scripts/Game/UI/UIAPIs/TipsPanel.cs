using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : HTBehaviour
{
    public Text _Title;
    public Text _Content;

    public Button _CloseButton;
    public GameObject _RootPanel;

    private void Start()
    {
        _CloseButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    public void Hide()
    {
        _RootPanel.transform.DOScale(0, 0.3f)
            .SetEase(Ease.OutExpo);
        Invoke(nameof(Close), 0.3f);
    }

    public void Close()
    {
        Main.m_UI.CloseUI<TipsUILogic>();
    }

    public void Show(string content, string title = "提示：", float width = 200)
    {
        _Title.text = title;
        _Content.text = content;
        _RootPanel.SetFloatWithAnimation(this);
        _RootPanel.rectTransform().sizeDelta = new Vector2(width, 150);
    }
}
