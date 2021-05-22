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

    public void ShowIndicate(string title, string desc)
    {
        Title.text = title;
        Desc.text = desc;
        gameObject.rectTransform().SetFloat();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
