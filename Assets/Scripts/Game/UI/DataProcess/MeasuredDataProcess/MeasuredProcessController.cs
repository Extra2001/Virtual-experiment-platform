using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuredProcessController : HTBehaviour
{
    public Text _title;
    public GameObject _navigationBar;
    public Text _navigationTitle;
    public Button _backButton;
    public Button _continueButton;

    public Button _tableButton;
    public Button _differenceButton;
    public Button _regressionButton;
    public Button _graphicButton;

    public GameObject _chooserPanel;
    public DataColumn dataColumn1;
    public DataColumn dataColumn2;
    public MeasuredUncertainty measuredUncertainty;
    public MeasuredDifference1 measuredDifference1;
    public MeasuredDifference2 measuredDifference2;
    public MeasuredRegression1 measuredRegression1;
    public MeasuredRegression2 measuredRegression2;

    private QuantityModel quantity;

    private void Start()
    {
        _backButton.onClick.AddListener(BackButton);
        _continueButton.onClick.AddListener(ContinueButton);
        _tableButton.onClick.AddListener(ShowUncertainty);
        _differenceButton.onClick.AddListener(ShowDifference1);
        _regressionButton.onClick.AddListener(ShowRegression1);
    }

    public void Initialize(QuantityModel quantity)
    {
        HideAllPanel();
        this.quantity = quantity;
        _chooserPanel.gameObject.SetActive(true);
        ShowDatatable(dataColumn1, DataColumnType.Mesured, quantity.MesuredData);
        var instance = quantity.InstrumentType.CreateInstrumentInstance();
        _title.text = "处理" + quantity.Name + ":" + quantity.Symbol + "/" + instance.UnitSymbol;
    }

    private void BackButton()
    {
        if (measuredUncertainty.gameObject.activeSelf || measuredDifference1.gameObject.activeSelf || measuredRegression1.gameObject.activeSelf)
            Initialize(quantity);
        else if (measuredDifference2.gameObject.activeSelf)
            ShowDifference1();
        else if (measuredRegression2.gameObject.activeSelf)
            ShowRegression1();
    }

    private void ContinueButton()
    {
        if (measuredDifference1.gameObject.activeSelf)
            ShowDifference2();
        else if (measuredRegression1.gameObject.activeSelf)
            ShowRegression2();
    }

    private void ShowDatatable(DataColumn dataColumn, DataColumnType type, DataColumnModel data, bool readOnly = true)
    {
        dataColumn.gameObject.SetActive(true);
        dataColumn.SetClass(type);
        dataColumn.Show(data, true);
        dataColumn.ReadOnly = readOnly;
        dataColumn.Changable = false;
        dataColumn.Deletable = false;
    }

    private void HideAllPanel()
    {
        _navigationBar.SetActive(false);
        dataColumn1.gameObject.SetActive(false);
        dataColumn2.gameObject.SetActive(false);
        measuredUncertainty.gameObject.SetActive(false);
        measuredDifference1.gameObject.SetActive(false);
        measuredDifference2.gameObject.SetActive(false);
        measuredRegression1.gameObject.SetActive(false);
        measuredRegression2.gameObject.SetActive(false);
        _chooserPanel.gameObject.SetActive(false);
        _backButton.interactable = false;
        _continueButton.interactable = false;
    }

    private void ShowNavigationBar(string title, int size)
    {
        _navigationBar.SetActive(true);
        _navigationTitle.text = title;
        var sized = _navigationBar.rectTransform().sizeDelta;
        if (size == 1) sized.x = 750;
        else if (size == 2) sized.x = 550;
        else if (size == 3) sized.x = 350;
        _navigationBar.rectTransform().sizeDelta = sized;
        var position = _navigationBar.transform.localPosition;
        if (size == 1) position.x = 0;
        else if (size == 2) position.x = 100;
        else if (size == 3) position.x = 200;
        _navigationBar.transform.localPosition = position;
    }

    private void ShowUncertainty()
    {
        HideAllPanel();
        ShowDatatable(dataColumn1, DataColumnType.Mesured, quantity.MesuredData);
        ShowNavigationBar("直接计算不确定度", 2);
        measuredUncertainty.gameObject.SetActive(true);
        measuredUncertainty.Show(quantity);
        _backButton.interactable = true;
    }

    private void ShowDifference1()
    {
        HideAllPanel();
        ShowDatatable(dataColumn1, DataColumnType.Mesured, quantity.MesuredData);
        ShowDatatable(dataColumn2, DataColumnType.Differenced, quantity.DifferencedData, false);
        ShowNavigationBar("逐差法第一步", 3);
        measuredDifference1.gameObject.SetActive(true);
        measuredDifference1.Show(quantity);
        _backButton.interactable = true;
        _continueButton.interactable = true;
    }

    private void ShowDifference2()
    {
        HideAllPanel();
        ShowDatatable(dataColumn1, DataColumnType.Differenced, quantity.DifferencedData);
        ShowNavigationBar("逐差法第二步", 2);
        measuredDifference2.gameObject.SetActive(true);
        measuredDifference2.Show(quantity);
        _backButton.interactable = true;
    }

    private void ShowRegression1()
    {
        HideAllPanel();
        ShowDatatable(dataColumn1, DataColumnType.Differenced, quantity.DifferencedData);
        ShowDatatable(dataColumn2, DataColumnType.Independent, quantity.IndependentData, false);
        ShowNavigationBar("一元线性回归", 3);
        measuredRegression1.gameObject.SetActive(true);
        measuredRegression1.Show(quantity);
        _backButton.interactable = true;
        _continueButton.interactable = true;
    }

    private void ShowRegression2()
    {
        HideAllPanel();
        ShowNavigationBar("一元线性回归法第二步", 1);
        measuredRegression2.gameObject.SetActive(true);
        measuredRegression2.Show(quantity);
        _backButton.interactable = true;
    }
}
