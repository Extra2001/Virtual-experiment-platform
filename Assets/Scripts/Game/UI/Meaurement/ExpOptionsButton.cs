using HT.Framework;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExpOptionsButton : HTBehaviour
{
    public Button _Options;
    public GameObject _RootPanel;
    public Button _Close;
    public Button _SetValue;
    public Button _SetExpression;
    public Button _Preview;

    private void Start()
    {
        _Options.onClick.AddListener(() =>
        {
            if (_Close.gameObject.activeSelf)
                Hide();
            else Show();
        });
        _Close.onClick.AddListener(Hide);
        _SetValue.onClick.AddListener(() =>
        {
            Main.m_UI.OpenUI<AddValueUILogic>(true);
            Hide();
        });
        _SetExpression.onClick.AddListener(() =>
        {
            Main.m_UI.OpenUI<EnterExpressionUILogic>(true);
            Hide();
        });
        _Preview.onClick.AddListener(() =>
        {
            Main.m_UI.OpenUI<PreviewUILogic>(true);
            Hide();
        });
    }

    private void Hide()
    {
        _RootPanel.transform.DOScale(0, 0.3f)
            .SetEase(Ease.OutExpo);
        Invoke(nameof(Close), 0.3f);
    }

    private void Close()
    {
        _Close.gameObject.SetActive(false);
        _RootPanel.SetActive(false);
    }

    private void Show()
    {
        _Close.gameObject.SetActive(true);
        _RootPanel.SetActive(true);
        _RootPanel.GetComponentsInChildren<Button>(true).Foreach((x, _) =>
        {
            x.gameObject.SetActive(true);
        });
        _RootPanel.SetFloatWithAnimation(this);
    }
}
