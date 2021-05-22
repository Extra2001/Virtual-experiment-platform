using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class FormulaController : MonoBehaviour
{
    /// <summary>
    /// 获取实例对象
    /// </summary>
    public static Dictionary<string, FormulaController> Instances { get; } = new Dictionary<string, FormulaController>();
    /// <summary>
    /// 获取当前的表达式，如果未完成填写会抛异常
    /// </summary>
    public string Expression { get => GetExpression("base"); }
    /// <summary>
    /// 获取当前表达式计算值
    /// </summary>
    public double ExpressionExecuted { get => Javascript.Eval(GetExpression("base")); }
    /// <summary>
    /// 实例名称
    /// </summary>
    public string InstanceName;

    [SerializeField]
    private FormulaCell baseCell;
    [SerializeField]
    private GameObject Selector;
    [SerializeField]
    private GameObject MeasuredSelector;
    [SerializeField]
    private GameObject ComplexSelector;
    [SerializeField]
    private GameObject Mask;
    [SerializeField]
    public FormulaIndicator Indicator;
    private List<FormulaCell> showedCells = new List<FormulaCell>();
    private Button clickedButton;
    private FormulaCell clickedCell;

    public delegate void OnSelectCell();

    public event OnSelectCell onSelectCell;

    private void Start()
    {
        CheckActive();
    }

    /// <summary>
    /// 在公式选择器里选择节点
    /// </summary>
    /// <param name="cellName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public FormulaCell SelectCell(string cellName, string value = "0", string name = "x")
    {
        HideSelector();
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
                ShowSelector();
            });
        if (hh.Value2 != null)
            hh.Value2.onClick.AddListener(() =>
            {
                clickedButton = hh.Value2;
                clickedCell = hh;
                ShowSelector();
            });
        if (cellName.Equals("Customize"))
        {
            hh.value = value;
            if (hh.NameShower)
                hh.NameShower.text = value;
        }
        else if (cellName.StartsWith("Statistic"))
        {
            hh.value = value;
            if (hh.NameShower)
                hh.NameShower.text = name;
        }
        RefreshContentSizeFitter();
        onSelectCell?.Invoke();
        return hh;
    }

    /// <summary>
    /// 获取序列化后的公式节点
    /// </summary>
    /// <returns></returns>
    public List<FormulaNode> Serialize()
    {
        CheckActive();
        var ret = new List<FormulaNode>();
        foreach (var item in showedCells)
        {
            if (!item)
                continue;
            var node = new FormulaNode()
            {
                value = item.value,
                GUID = item.thisGUID,
                PrefabName = item.gameObject.name.Replace("(Clone)", ""),
                ReplaceFlags = item.ReplaceFlags.Select(x => x.Value).ToList()
            };
            ret.Add(node);
        }
        return ret;
    }

    /// <summary>
    /// 加载序列化后的节点，调用此函数会导致本控制器下输入的公式被清空。
    /// </summary>
    /// <param name="nodes"></param>
    public void LoadFormula(List<FormulaNode> nodes)
    {
        CheckActive();
        if (nodes.Count == 0 || nodes[0].PrefabName != "Base")
        {
            Log.Error("加载的公式节点并非控制器产生");
            Initialize();
        }
        // 清空当前显示
        for (int i = 1; i < showedCells.Count; i++)
            Destroy(showedCells[i]?.gameObject);
        for (int i = 1; i < showedCells.Count; i++)
            showedCells.RemoveAt(i);
        Log.Info(showedCells.Count.ToString());
        RefreshContentSizeFitter();

        // 处理根节点
        var copyNodes = nodes.DeepCopy<List<FormulaNode>>();
        var baseNode = copyNodes[0];
        copyNodes.RemoveAt(0);
        baseCell.value = baseNode.value;
        baseCell.ReplaceFlags.Clear();
        if (baseNode.ReplaceFlags.Count >= 1)
            baseCell.ReplaceFlags.Add(baseCell.Value1, baseNode.ReplaceFlags[0]);
        if (baseNode.ReplaceFlags.Count >= 2)
            baseCell.ReplaceFlags.Add(baseCell.Value2, baseNode.ReplaceFlags[1]);
        baseCell.thisGUID = baseNode.GUID;

        // 显示方块
        RestoreCell(baseCell, nodes);
    }

    /// <summary>
    /// 初始化公式编辑器，此操作会清空其中所有内容。
    /// </summary>
    public void Initialize()
    {
        AddInstance();
        baseCell.ReplaceFlags.Clear();
        baseCell.ReplaceFlags.Add(baseCell.Value1, "{0}");
        DeleteChild(baseCell.Value1.transform);
        showedCells.Clear();
        showedCells.Add(baseCell);
        clickedButton = baseCell.Value1;
        clickedCell = baseCell;
        Indicator.Hide();
        this.Mask.GetComponent<Button>().onClick.AddListener(() =>
        {
            HideSelector();
        });
        baseCell.Value1.onClick.AddListener(() =>
        {
            clickedButton = baseCell.Value1;
            clickedCell = baseCell;
            ShowSelector();
        });

        var v = Selector.transform.position;
        v.y = 1000;
        Selector.transform.position = v;

        v = MeasuredSelector.transform.position;
        v.y = -1000;
        MeasuredSelector.transform.position = v;

        v = ComplexSelector.transform.position;
        v.y = -1000;
        ComplexSelector.transform.position = v;

        Selector.SetActive(true);
        MeasuredSelector.SetActive(true);
        ComplexSelector.SetActive(true);

        Mask.SetActive(false);
        onSelectCell?.Invoke();
    }

    private void CheckActive()
    {
        if (showedCells.Count == 0)
            Initialize();
    }

    /// <summary>
    /// 从节点恢复方块
    /// </summary>
    private void RestoreCell(FormulaCell cell, List<FormulaNode> nodes)
    {
        foreach (var item in cell.ReplaceFlags)
        {
            var node = nodes.Where(x => x.GUID.Equals(item.Value)).FirstOrDefault();
            if (node == null)
                continue;
            clickedButton = item.Key;
            clickedCell = cell;
            var newCell = SelectCell(node.PrefabName, node.value);
            newCell.value = node.value;
            newCell.ReplaceFlags.Clear();
            if (node.ReplaceFlags.Count >= 1)
                newCell.ReplaceFlags.Add(newCell.Value1, node.ReplaceFlags[0]);
            if (node.ReplaceFlags.Count >= 2)
                newCell.ReplaceFlags.Add(newCell.Value2, node.ReplaceFlags[1]);
            newCell.thisGUID = node.GUID;
            RestoreCell(newCell, nodes);
        }
    }

    private void ShowSelector()
    {
        Mask.SetActive(true);
        var v = Selector.transform.position;
        v.x = 0;
        Selector.transform.position = v;
        UIShowHideHelper.ShowFromUp(Selector, 0);
        if (Main.m_Procedure.CurrentProcedure is MeasuredDataProcessProcedure)
        {
            var procedure = Main.m_Procedure.CurrentProcedure as MeasuredDataProcessProcedure;
            foreach (var item in MeasuredSelector.GetComponentsInChildren<FormulaSelectorCell>())
                item.SetSelectorName(procedure.GetStatisticValue(MeasuredStatisticValue.Symbol));
            UIShowHideHelper.ShowFromButtom(MeasuredSelector, 0);
        }
        else if (Main.m_Procedure.CurrentProcedure is ComplexDataProcessProcedure)
            UIShowHideHelper.ShowFromButtom(ComplexSelector, 0);
    }

    private void HideSelector()
    {
        Mask.SetActive(false);
        UIShowHideHelper.HideToUp(Selector);
        if (Main.m_Procedure.CurrentProcedure is MeasuredDataProcessProcedure)
            UIShowHideHelper.HideToButtom(MeasuredSelector);
        else if (Main.m_Procedure.CurrentProcedure is ComplexDataProcessProcedure)
            UIShowHideHelper.HideToButtom(ComplexSelector);
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
        var hh = cell.gameObject.GetComponentsInChildren<FormulaCell>(true);
        for (int i = 1; i < hh.Length; i++)
            showedCells.Remove(hh[i]);
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

    private void AddInstance()
    {
        if (string.IsNullOrEmpty(InstanceName))
        {
            for (int i = 0; string.IsNullOrEmpty(InstanceName); i++)
                if (!Instances.ContainsKey(i.ToString()))
                    InstanceName = i.ToString();
        }
        Instances.Remove(InstanceName);
        Instances.Add(InstanceName, this);

        foreach (var item in Selector.GetComponentsInChildren<FormulaSelectorCell>(true))
            item.FormulaControllerInstance = this;
    }

    private void OnDestroy()
    {
        Instances.Remove(InstanceName);
    }
}
