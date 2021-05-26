using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormulaIndicator : HTBehaviour
{
    public Text Title;
    public Text Desc;
    public Text Value;

    public void ShowIndicate(string title, string desc, string value)
    {
        Title.text = title;
        Desc.text = desc;
        Value.text = value;
        gameObject.rectTransform().SetFloat();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
