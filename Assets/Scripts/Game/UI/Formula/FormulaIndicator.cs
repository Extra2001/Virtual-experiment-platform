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
        var hh = Input.mousePosition;
        print(hh);
        hh.x += gameObject.rectTransform().rect.width * (hh.x > Screen.width / 2 ? -1 : 1);
        hh.y += gameObject.rectTransform().rect.height * (hh.y > Screen.height / 2 ? -1 : 1);
        transform.position = hh;
        gameObject.SetActive(true);
        Title.text = title;
        Desc.text = desc;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
