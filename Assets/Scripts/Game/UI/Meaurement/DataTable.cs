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

    private List<DataColumn> showedColumn = new List<DataColumn>();
    private int times = 0;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            UIAPI.Instance.HideDataTable();
        });
    }
    /// <summary>
    /// 将物理量显示在数据表格上
    /// </summary>
    /// <param name="filter"></param>
    private void Show(Func<Type, bool> filter)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.rectTransform());
        foreach (var item in showedColumn)
            Destroy(item.gameObject);
        showedColumn.Clear();
        foreach (var item in RecordManager.tempRecord.quantities)
        {
            if (filter.Invoke(item.InstrumentType))
            {
                var showed = Instantiate(_dataColumn, _ColumnContainer.transform);
                showed.ShowQuantity(item);
                showedColumn.Add(showed);
            }
        }
        times++;
        if (times < 3)
            Show(filter);
        var tmp = _Content.GetComponent<ContentSizeFitter>();
        tmp.horizontalFit = ContentSizeFitter.FitMode.MinSize;
        tmp = _Content.transform.GetChild(0).GetComponent<ContentSizeFitter>();
        tmp.horizontalFit = ContentSizeFitter.FitMode.MinSize;
    }

    public void Show(Type instrumentType)
    {
        Show(x => instrumentType.Equals(x));
    }

    public void Show()
    {
        Show(_ => true);
    }
}
