using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormulaCustomizeCell : HTBehaviour
{
    public Text show;
    public FormulaSelectorCell FormulaSelectorCell;
    public InputField input;
    
    // Start is called before the first frame update
    void Start()
    {
        input.onValueChanged.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                FormulaSelectorCell.value = $"(0)";
                show.text = "0";
                return;
            }
            FormulaSelectorCell.value = $"({value})";
            show.text = value;
        });
    }
}
