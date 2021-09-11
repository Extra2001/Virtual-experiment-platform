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
        // �����������ı��
        if (quantity.MesuredData.data.Count / 2 != quantity.DifferencedData.data.Count)
        {
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                ShowCancel = false,
                Message = new BindableString("���������������������ȷ")
            });
            return false;
        }
        // �������鲽����û��д
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
