using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuredRegression1 : HTBehaviour
{
    public Image previewImage;
    public Dropdown dataset;
    public Dropdown nextValue;
    private QuantityModel quantity;

    private void Start()
    {
        dataset.onValueChanged.AddListener(x => quantity.dataset = x);
        nextValue.onValueChanged.AddListener(x => quantity.nextValue = x);
    }

    public void Show(QuantityModel quantity)
    {
        this.quantity = quantity;
        ShowImage();
        dataset.options[0].text = quantity.MesuredData.name;
        dataset.options[1].text = quantity.IndependentData.name;
        dataset.SetValueWithoutNotify(quantity.dataset);
        nextValue.SetValueWithoutNotify(quantity.nextValue);
    }

    public bool CheckAll()
    {
        if (quantity.MesuredData.data.Count != quantity.IndependentData.data.Count)
        {
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                ShowCancel = false,
                Message = new BindableString("自变量数据组数与因变量不相等")
            });
            return false;
        }
        return true;
    }

    private void ShowImage()
    {
        LatexEquationRender.Render("y=a+bx", previewImage.FitWidth);
    }
}
