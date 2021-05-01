using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FormulaController : HTBehaviour
{
    /// <summary>
    /// 获取实例对象
    /// </summary>
    public static FormulaController Instance { get; private set; }
    /// <summary>
    /// 获取当前的表达式，如果未完成填写会抛异常
    /// </summary>
    public string Expression { get => GetExpression("base"); }
    /// <summary>
    /// 获取当前表达式计算值
    /// </summary>
    public double ExpressionExecuted { get => Javascript.Eval(GetExpression("base")); }

    [SerializeField]
    private FormulaCell baseCell;
    private List<FormulaCell> showedCells = new List<FormulaCell>();
    private Button clickedButton;
    private FormulaCell clickedCell;

    private void Start()
    {
        Instance = this;
        baseCell.ReplaceFlags.Add(baseCell.Value1, "{0}");
        showedCells.Add(baseCell);
        clickedButton = baseCell.Value1;
        clickedCell = baseCell;
        baseCell.Value1.onClick.AddListener(() =>
        {
            clickedButton = baseCell.Value1;
            clickedCell = baseCell;

            // 呼出选择器
        });
    }

    public void SelectCell(string cellName, string value = "0")
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
        if (cellName.Equals("Customize")) HandleCustomize(hh, value);
        RefreshContentSizeFitter();
    }

    private void HandleCustomize(FormulaCell cellInstance, string value)
    {
        cellInstance.value = value;
        cellInstance.gameObject.GetComponent<FormulaCustomizeShower>().SetValue(value);
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
        for (int i = 0; i < childCnt; i++)
            Destroy(cell.transform.GetChild(i).gameObject);
    }

    private void RefreshContentSizeFitter()
    {
        var list = showedCells[0].GetComponentsInChildren<ContentSizeFitter>();
        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        LayoutRebuilder.ForceRebuildLayoutImmediate(showedCells[0].gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        LayoutRebuilder.ForceRebuildLayoutImmediate(showedCells[0].gameObject.rectTransform());
        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        LayoutRebuilder.ForceRebuildLayoutImmediate(showedCells[0].gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        LayoutRebuilder.ForceRebuildLayoutImmediate(showedCells[0].gameObject.rectTransform());
    }

    private string GetExpression(string guid)
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
