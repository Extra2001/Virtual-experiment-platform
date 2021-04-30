using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormulaCustomizeShower : HTBehaviour
{
    [SerializeField]
    private Text Value;

    public void SetValue(string value)
    {
        Value.text = value;
    }
}
