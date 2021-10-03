using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        if (quantity.IndependentData == null) quantity.IndependentData = new DataColumnModel()
        {
            name = $"[自] 自变量 {quantity.Name}",
            quantitySymbol = quantity.Symbol,
            type = DataColumnType.Independent
        };
        this.quantity = quantity;
        ShowImage();
        dataset.options[0].text = quantity.MesuredData.name;
        dataset.options[1].text = quantity.IndependentData.name;
        dataset.SetValueWithoutNotify(quantity.dataset);
        nextValue.SetValueWithoutNotify(quantity.nextValue);
    }

    public bool CheckAll(bool silent = false)
    {
        if (quantity.MesuredData.data.Count != quantity.IndependentData.data.Count)
        {
            if (!silent)
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "自变量数据组数与因变量不相等"
                });
            return false;
        }
        if (quantity.IndependentData.data.Where(x => string.IsNullOrEmpty(x)).Count() > 0)
        {
            if (!silent)
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "自变量数据有空值"
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
