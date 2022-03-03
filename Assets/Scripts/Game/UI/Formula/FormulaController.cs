/************************************************************************************
    作者：荆煦添
    描述：公式编辑器中央控制器
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public partial class FormulaController : MonoBehaviour
{
    /// <summary>
    /// 全局开关
    /// </summary>
    public bool interactable
    {
        get => !EnabledMask.activeSelf;
        set
        {
            EnabledMask.SetActive(!value);
            Selector.SetActive(value);
            MeasuredSelector.SetActive(value);
            ComplexSelector.SetActive(value);
        }
    }
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
    #region Unity序列化配置
    [SerializeField]
    private FormulaCell baseCell;
    [SerializeField]
    private GameObject Selector;
    [SerializeField]
    private GameObject MeasuredSelector;
    [SerializeField]
    private GameObject MeasuredUncertainty;
    [SerializeField]
    private GameObject MeasuredDifference;
    [SerializeField]
    private GameObject MeasuredRegression;
    [SerializeField]
    private GameObject ComplexSelector;
    [SerializeField]
    private GameObject Mask;
    [SerializeField]
    public FormulaIndicator Indicator;
    [SerializeField]
    private GameObject ComplexPanel;
    [SerializeField]
    private Transform ComplexPanelRoot;
    [SerializeField]
    private GameObject EnabledMask;
    #endregion

    private List<FormulaCell> showedCells = new List<FormulaCell>();
    private Button clickedButton;
    private FormulaCell clickedCell;

    public delegate void OnSelectCell();
    public event OnSelectCell onSelectCell;

    private void Start()
    {
        Mask.GetComponent<Button>().onClick.AddListener(HideSelector);
        AddClickListener(baseCell, baseCell.Value1);
        CheckActive();
    }
    /// <summary>
    /// 初始化公式编辑器，此操作会清空其中所有内容。
    /// </summary>
    public void Initialize()
    {
        // 清空根节点下的替换内容
        baseCell.ReplaceFlags.Clear();
        baseCell.ReplaceFlags.Add(baseCell.Value1, "{0}");
        baseCell.value = "{0}";
        // 清空保存的已显示的方块
        showedCells.Clear();
        showedCells.Add(baseCell);
        DeleteChild(baseCell, baseCell.Value1);
        // 重新设置当前为根节点
        clickedButton = baseCell.Value1;
        clickedCell = baseCell;

        // 隐藏一些乱七八糟的东西
        Indicator.Hide();
        HideSelector();
        Selector.SetActive(true);
        MeasuredSelector.SetActive(true);
        ComplexSelector.SetActive(true);
        Mask.SetActive(false);
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
            if (!item) continue;
            var node = new FormulaNode()
            {
                value = item.value,
                GUID = item.thisGUID,
                PrefabName = item.gameObject.name.Replace("(Clone)", ""),
                ReplaceFlags = item.ReplaceFlags.Select(x => x.Value).ToList(),
                name = item.NameShower?.text
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
            return;
        }
        // 清空当前显示
        Initialize();
        if (baseCell.gameObject.activeInHierarchy) StartCoroutine(RefreshContentSize(baseCell.gameObject));
        else RefreshContentSizeFitter(baseCell.gameObject);
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
        RestoreCell(baseCell, copyNodes);
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
        DeleteChild(clickedCell, clickedButton);
        var newCell = Instantiate(cell, clickedButton.gameObject.transform);
        newCell.thisGUID = clickedCell.ReplaceFlags[clickedButton];
        newCell.GenerateReplaceFlags();
        showedCells.Add(newCell);
        AddClickListener(newCell, newCell.Value1);
        AddClickListener(newCell, newCell.Value2);
        if (cellName.Equals("Customize"))
        {
            newCell.value = value;
            if (newCell.NameShower) newCell.NameShower.text = value;
        }
        else if (cellName.StartsWith("Statistic"))
        {
            newCell.value = value;
            if (newCell.NameShower) newCell.NameShower.text = name;
        }
        if (baseCell.gameObject.activeInHierarchy) StartCoroutine(RefreshContentSize(baseCell.gameObject));
        else RefreshContentSizeFitter(baseCell.gameObject);
        onSelectCell?.Invoke();
        return newCell;
    }

    private void AddClickListener(FormulaCell formulaCell, Button button)
    {
        if (button != null)
            button.onClick.AddListener(() =>
            {
                clickedButton = button;
                clickedCell = formulaCell;
                ShowSelector();
            });
    }
    /// <summary>
    /// 检查节点是否存在
    /// </summary>
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
            var newCell = SelectCell(node.PrefabName, node.value, node.name);
            newCell.value = node.value;
            newCell.ReplaceFlags.Clear();
            if (node.ReplaceFlags.Count >= 1)
                newCell.ReplaceFlags.Add(newCell.Value1, node.ReplaceFlags[0]);
            if (node.ReplaceFlags.Count >= 2)
                newCell.ReplaceFlags.Add(newCell.Value2, node.ReplaceFlags[1]);
            newCell.thisGUID = node.GUID;
            if ((!string.IsNullOrEmpty(node.name)) && (newCell.NameShower))
                newCell.NameShower.text = node.name;
            RestoreCell(newCell, nodes);
        }
    }
    /// <summary>
    /// 显示选择器
    /// </summary>
    private void ShowSelector()
    {
        Mask.SetActive(true);
        UIShowHideHelper.ShowFromUp(Selector, 0);
        if (Main.m_Procedure.CurrentProcedure is MeasuredDataProcessProcedure)
        {
            // 获取方块的值
            var procedure = Main.m_Procedure.CurrentProcedure as MeasuredDataProcessProcedure;
            foreach (var item in MeasuredSelector.GetComponentsInChildren<FormulaSelectorCell>(true))
                item.SetSelectorName(procedure.GetStatisticValue(MeasuredStatisticValue.Symbol));
            // 逐差、线性回归的独特方法
            MeasuredDifference.SetActive(false);
            MeasuredRegression.SetActive(false);
            if (GameManager.Instance.CurrentQuantity.processMethod == 2)
                MeasuredDifference.SetActive(true);
            else if (GameManager.Instance.CurrentQuantity.processMethod == 3)
                MeasuredRegression.SetActive(true);
            // 刷新并显示
            RefreshContentSizeFitter(MeasuredSelector.gameObject);
            UIShowHideHelper.ShowFromButtom(MeasuredSelector, 0);
        }
        else if (Main.m_Procedure.CurrentProcedure is ComplexDataProcessProcedure)
        {
            // 获取方块的值
            var procedure = Main.m_Procedure.CurrentProcedure as ComplexDataProcessProcedure;
            for (int i = 0; i < ComplexPanelRoot.childCount; i++)
                Destroy(ComplexPanelRoot.GetChild(i).gameObject);
            foreach (var item in procedure.GetQuantitiesName())
                Instantiate(ComplexPanel, ComplexPanelRoot).GetComponent<FormulaComplexSelectorCell>().Show(item, this);
            // 刷新并显示
            RefreshContentSizeFitter(ComplexPanelRoot.gameObject);
            UIShowHideHelper.ShowFromButtom(ComplexSelector, 0);
        }
    }
    /// <summary>
    /// 隐藏选择器
    /// </summary>
    private void HideSelector()
    {
        Mask.SetActive(false);
        UIShowHideHelper.HideToUp(Selector);
        UIShowHideHelper.HideToButtom(MeasuredSelector);
        UIShowHideHelper.HideToButtom(ComplexSelector);
    }
    /// <summary>
    /// 获取方块对象
    /// </summary>
    private FormulaCell GetCell(string cellName)
    {
        var obj = Resources.Load<GameObject>($"UI/Formula/Cells/{cellName}");
        return obj.GetComponent<FormulaCell>();
    }
    /// <summary>
    /// 删除该节点从属的所有节点
    /// </summary>
    private void DeleteChild(FormulaCell cell, Button button)
    {
        int childCnt = button.gameObject.transform.childCount;
        var cells = button.gameObject.GetComponentsInChildren<FormulaCell>(true)
            .Where(x => x.thisGUID != cell.thisGUID).ToList();
        // 从记录中删除
        foreach (var item in cells)
        {
            var find = showedCells.Find(x => x.thisGUID.Equals(item.thisGUID));
            if (find != null) showedCells.Remove(find);
        }
        // 销毁GameObject
        for (int i = 0; i < childCnt; i++)
            Destroy(button.transform.GetChild(i).gameObject);
        if (baseCell.gameObject.activeInHierarchy) StartCoroutine(RefreshContentSize(baseCell.gameObject));
        else RefreshContentSizeFitter(baseCell.gameObject);
    }
    private IEnumerator RefreshContentSize(GameObject baseObject)
    {
        var list = baseObject.GetComponentsInChildren<ContentSizeFitter>();
        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());

        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());

        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        yield return 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        //RefreshContentSizeFitter(baseObject);
        //yield return 1;
        //RefreshContentSizeFitter(baseObject);
        //yield return 1;
        //RefreshContentSizeFitter(baseObject);
        //yield return 1;
    }
    /// <summary>
    /// 刷新UI适应布局
    /// </summary>
    /// <param name="baseObject"></param>
    public void RefreshContentSizeFitter(GameObject baseObject)
    {
        var list = baseObject.GetComponentsInChildren<ContentSizeFitter>();
        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = 0; i < list.Length; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
        for (int i = list.Length - 1; i >= 0; i--)
            LayoutRebuilder.ForceRebuildLayoutImmediate(list[i].gameObject.rectTransform());
        LayoutRebuilder.ForceRebuildLayoutImmediate(baseCell.gameObject.rectTransform());
    }
    /// <summary>
    /// 递归获取当前表达式
    /// </summary>
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
