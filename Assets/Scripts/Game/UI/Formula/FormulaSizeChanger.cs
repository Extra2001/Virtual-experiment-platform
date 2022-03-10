using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormulaSizeChanger : HTBehaviour
{
    public RectTransform FormulaRootPanel;
    public Button ChangeSizeButton;
    public Sprite ExpandSprite;
    public Sprite CollapseSprite;

    private Vector2 min;
    private Vector2 max;

    private bool Expanded = false;

    private void Start()
    {
        
        ChangeSizeButton.onClick.AddListener(() =>
        {
            if (Expanded)
                Collapse();
            else
                Expand();
        });
    }

    private void Expand()
    {
        Expanded = true;
        ChangeSizeButton.gameObject.GetComponent<Image>().sprite = CollapseSprite;

        var color = FormulaRootPanel.gameObject.GetComponent<Image>().color;
        color.a = 0.784f;
        FormulaRootPanel.gameObject.GetComponent<Image>().DOColor(color, 0.3f)
            .SetEase(Ease.OutExpo);

        min = FormulaRootPanel.offsetMin;
        max = FormulaRootPanel.offsetMax;

        DOTween.To(() => FormulaRootPanel.offsetMin, x => FormulaRootPanel.offsetMin = x, new Vector2(0, 0), 0.3f)
            .SetEase(Ease.OutExpo);
        DOTween.To(() => FormulaRootPanel.offsetMax, x => FormulaRootPanel.offsetMax = x, new Vector2(0, 0), 0.3f)
            .SetEase(Ease.OutExpo);
    }

    private void Collapse()
    {
        Expanded = false;
        ChangeSizeButton.gameObject.GetComponent<Image>().sprite = ExpandSprite;

        var color = FormulaRootPanel.gameObject.GetComponent<Image>().color;
        color.a = 0.392f;
        //FormulaRootPanel.gameObject.GetComponent<Image>().color = color;
        FormulaRootPanel.gameObject.GetComponent<Image>().DOColor(color, 0.3f)
            .SetEase(Ease.OutExpo);

        DOTween.To(() => FormulaRootPanel.offsetMin, x => FormulaRootPanel.offsetMin = x, min, 0.3f)
            .SetEase(Ease.OutExpo);
        DOTween.To(() => FormulaRootPanel.offsetMax, x => FormulaRootPanel.offsetMax = x, max, 0.3f)
            .SetEase(Ease.OutExpo);
    }
}
