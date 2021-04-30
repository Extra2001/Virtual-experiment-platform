using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FormulaController : HTBehaviour
{
    public static FormulaController Instance { get; private set; }

    public FormulaCell baseCell;

    private List<FormulaCell> showedCells = new List<FormulaCell>();

    private Button clickedButton;

    private FormulaCell clickedCell;

    public string Expression { get => GetExpression("base"); }

    private void Start()
    {
        Instance = this;
        baseCell.thisGUID = "base";
        showedCells.Add(baseCell);
        baseCell.Value1.onClick.AddListener(() =>
        {
            clickedButton = baseCell.Value1;
            clickedCell = baseCell;

            // 呼出选择器
        });
    }

    private FormulaCell GetCell(string cellName)
    {
        var obj = Resources.Load<GameObject>($"UI/Formula/Cells/{cellName}");
        var ret = obj.GetComponent<FormulaCell>();
        return ret;
    }

    private void DeleteChild(Transform cell)
    {
        int childCnt = cell.childCount;
        for(int i = 0; i < childCnt; i++)
            Destroy(cell.transform.GetChild(i).gameObject);
    }

    private void RefreshContentSizeFitter()
    {
        var list = showedCells[0].GetComponentsInChildren<ContentSizeFitter>();
        foreach(var item in list)
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.gameObject.rectTransform());
        foreach (var item in list)
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.gameObject.rectTransform());
        LayoutRebuilder.ForceRebuildLayoutImmediate(showedCells[0].gameObject.rectTransform());
    }

    public void SelectCell(string cellName)
    {
        var cell = GetCell(cellName);
        DeleteChild(clickedButton.transform);
        var hh = Instantiate(cell, clickedButton.gameObject.transform);
        hh.thisGUID = clickedCell.ReplaceFlags[clickedButton];
        hh.GenerateReplaceFlags();
        showedCells.Add(hh);
        if (hh.Value1 != null)
            hh.Value1.onClick.AddListener(() =>
            {
                clickedButton = hh.Value1;
                clickedCell = hh;

                // 呼出选择器
            });
        if (hh.Value2 != null)
            hh.Value2.onClick.AddListener(() =>
            {
                clickedButton = hh.Value2;
                clickedCell = hh;

                // 呼出选择器
            });
        RefreshContentSizeFitter();
    }

    public string GetExpression(string guid)
    {
        var curCell = showedCells.Where(x => x.thisGUID.Equals(guid)).Last();
        var value = curCell.value;
        foreach (var item in curCell.ReplaceFlags)
        {
            var subCell = showedCells.Where(x => x.thisGUID.Equals(item.Value)).Last();
            value = value.Replace(item.Value, $"({GetExpression(item.Value)})");
        }
        return value;
    }



}
