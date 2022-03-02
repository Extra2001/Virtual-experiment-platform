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
        if (quantity.DifferencedData == null) quantity.DifferencedData = new DataColumnModel()
        {
            name = $"[��] {quantity.Name} ({quantity.InstrumentType.CreateInstrumentInstance().UnitSymbol})",
            quantitySymbol = quantity.Symbol,
            type = DataColumnType.Differenced
        };
        ShowImage();
    }

    public bool CheckAll(bool silent = false)
    {
        // �����������ı��
        if (quantity.MesuredData.data.Count / 2 != quantity.DifferencedData.data.Count)
        {
            if (!silent)
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "���������������������ȷ"
                });
            return false;
        }

        if (quantity.MesuredData.data.Count / 2 < 2)
        {
            if (!silent)
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "��������������������٣������Ӳ�������"
                });
            return false;
        }

        if (!StaticMethods.CheckDifferenced(quantity.MesuredData.data, quantity.DifferencedData.data))
        {
            if (!silent)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "������������ݲ���ȷ"
                });
            }

            return false;
        }

        return true;
    }

    private void ShowImage()
    {
        var latex = $@"{quantity.Symbol}=\frac{{\delta {quantity.Symbol}}}{{{quantity.DifferencedData.data.Count}}}";
        LatexEquationRender.Render(latex, res => previewImage.FitImage(res, 170, 40));
    }

    private void OnDestroy()
    {
        controller.dataColumn2.onInputCountChange -= DataColumn2_onInputCountChange;
    }
}
