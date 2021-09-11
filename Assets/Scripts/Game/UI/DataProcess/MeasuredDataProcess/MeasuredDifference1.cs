using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MathNet.Symbolics;

public class MeasuredDifference1 : HTBehaviour
{
    public MeasuredProcessController controller;
    public InputField stepLength;
    public Image previewImage;

    private QuantityModel quantity;

    private void Start()
    {
        stepLength.onValueChanged.AddListener(x =>
        {
            if (string.IsNullOrEmpty(x)) return;
            quantity.stepLength = x;
            ShowImage();
        });
        controller.dataColumn2.onInputCountChange += DataColumn2_onInputCountChange;
    }

    private void DataColumn2_onInputCountChange()
    {
        ShowImage();
    }

    public void Show(QuantityModel quantity)
    {
        this.quantity = quantity;
        stepLength.text = quantity.stepLength;
        ShowImage();
    }

    public bool CheckAll()
    {
        // 在这里检查逐差法的表格
        if (quantity.MesuredData.data.Count / 2 != quantity.DifferencedData.data.Count)
        {
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                ShowCancel = false,
                Message = new BindableString("你的逐差表格的数据组数不正确")
            });
            return false;
        }
        // 在这里检查步长有没有写
        return true;
    }

    private void ShowImage()
    {
        var latex = $"{quantity.Symbol}=\\frac{{Z}}{{{quantity.stepLength}*{quantity.DifferencedData.data.Count}}}";
        LatexEquationRender.Render(latex, previewImage.FitHeight);
    }

    private void OnDestroy()
    {
        controller.dataColumn2.onInputCountChange -= DataColumn2_onInputCountChange;
    }
}
