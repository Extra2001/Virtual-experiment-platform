/************************************************************************************
    作者：荆煦添
    描述：数据记录表格
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataTable : HTBehaviour
{
    public DataColumn _dataColumn;
    public GameObject _ColumnContainer;
    public GameObject _Content;
    public Button _MaskButton;
    public Button _AddButton;
    public Button _MesuredButton;
    public Button _StepButton;
    public Button _DifferencedButton;

    private List<DataColumn> showedColumn = new List<DataColumn>();

    private void Start()
    {
        _MaskButton.onClick.AddListener(() =>
        {
            HideAllButtons();
            UIAPI.Instance.HideDataTable();
        });
        _AddButton.onClick.AddListener(() =>
        {
            if (_MesuredButton.gameObject.activeSelf)
                HideAllButtons();
            else
            {
                _MesuredButton.gameObject.SetActive(true);
                _MesuredButton.interactable = false;
                _StepButton.gameObject.SetActive(true);
                _StepButton.interactable = false;
                _DifferencedButton.gameObject.SetActive(true);
                _DifferencedButton.interactable = false;
                foreach (var item in RecordManager.tempRecord.quantities)
                {
                    if (item.MesuredData == null || (!item.MesuredData.addedToTable))
                        _MesuredButton.interactable = true;
                    if (item.IndependentData == null || (!item.IndependentData.addedToTable))
                        _StepButton.interactable = true;
                    if (item.DifferencedData == null || (!item.DifferencedData.addedToTable))
                        _DifferencedButton.interactable = true;
                }
            }
        });
        _MesuredButton.onClick.AddListener(() =>
        {
            showedColumn.Add(InstantiateDataColumn(DataColumnType.Mesured, true));
            HideAllButtons();
        });
        _StepButton.onClick.AddListener(() =>
        {
            showedColumn.Add(InstantiateDataColumn(DataColumnType.Independent, true));
            HideAllButtons();
        });
        _DifferencedButton.onClick.AddListener(() =>
        {
            showedColumn.Add(InstantiateDataColumn(DataColumnType.Differenced, true));
            HideAllButtons();
        });
    }

    private void HideAllButtons()
    {
        _MesuredButton.gameObject.SetActive(false);
        _StepButton.gameObject.SetActive(false);
        _DifferencedButton.gameObject.SetActive(false);
    }

    public void DeleteColumn(DataColumn dataColumn)
    {
        var index = showedColumn.FindIndex(x => x.GetInstanceID() == dataColumn.GetInstanceID());
        HideAllButtons();
        Show();
    }

    public void Show()
    {
        foreach (var item in showedColumn)
            Destroy(item.gameObject);
        showedColumn.Clear();
        foreach (var item in RecordManager.tempRecord.quantities)
        {
            if (item.MesuredData != null && item.MesuredData.addedToTable)
            {
                var cell = InstantiateDataColumn(DataColumnType.Mesured);
                cell.Show(item.MesuredData);
                showedColumn.Add(cell);
            }
            if (item.IndependentData != null && item.IndependentData.addedToTable)
            {
                var cell = InstantiateDataColumn(DataColumnType.Independent);
                cell.Show(item.IndependentData);
                showedColumn.Add(cell);
            }
            if (item.DifferencedData != null && item.DifferencedData.addedToTable)
            {
                var cell = InstantiateDataColumn(DataColumnType.Differenced);
                cell.Show(item.DifferencedData);
                showedColumn.Add(cell);
            }
        }
        HideAllButtons();
    }

    public void RefreshAllDropdown()
    {
        foreach (var item in showedColumn)
            item.RefreshDropdown();
    }

    private DataColumn InstantiateDataColumn(DataColumnType type, bool init = false)
    {
        var ret = Instantiate(_dataColumn, _ColumnContainer.transform);
        ret.dataTable = this;
        ret.SetClass(type, init);
        foreach(var item in GetComponentsInChildren<ContentSizeFitter>(true))
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.rectTransform());
        return ret;
    }
}