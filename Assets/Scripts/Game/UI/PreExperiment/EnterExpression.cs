/************************************************************************************
    作者：荆煦添
    描述：输入合成量表达式处理程序
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Events;

public class EnterExpression : HTBehaviour
{
    public GameObject _Content;
    public GameObject _QuantitySelectCell;
    public InputField StringExpressionInput;
    public Image image;
    public GameObject Loading;
    private List<GameObject> quantities = new List<GameObject>();

    void Start()
    {
        LoadQuantities();
        SyncExpression();
        StringExpressionInput.onValueChanged.AddListener(_ =>
        {
            RecordManager.tempRecord.stringExpression = StringExpressionInput.text;
            Render();
        });
    }

    /// <summary>
    /// 渲染回调加载图像
    /// </summary>
    /// <param name="sprite"></param>
    public void LoadSprite(Sprite sprite)
    {
        image.FitHeight(sprite);
    }
    /// <summary>
    /// 渲染表达式
    /// </summary>
    public void Render()
    {
        var text = StringExpressionInput.text;

        Task.Run(() =>
        {
            if (string.IsNullOrEmpty(text))
                LatexEquationRender.Render("expr.", LoadSprite);
            else
                LatexEquationRender.Render(CalcArgs.GetSymexpr(text).ToLaTeX(), LoadSprite);
        });
    }
    /// <summary>
    /// 异步校验表达式
    /// </summary>
    /// <param name="error"></param>
    public void Validate(UnityAction<bool> error)
    {
        var text = StringExpressionInput.text;
        Loading.SetActive(true);
        try
        {
            LatexEquationRender.Render(CalcArgs.GetSymexpr(text).ToLaTeX(),
                action: _ =>
                {
                    Loading.SetActive(false);
                    error?.Invoke(StaticMethods.ValidExpression(text));
                },
                errorHandler: () =>
                {
                    Loading.SetActive(false);
                    error?.Invoke(false);
                });
        }
        catch
        {
            Loading.SetActive(false);
            error?.Invoke(false);
        }
    }
    /// <summary>
    /// 同步表达式到存档
    /// </summary>
    public void SyncExpression()
    {
        StringExpressionInput.text = RecordManager.tempRecord.stringExpression;
        Render();
    }
    /// <summary>
    /// 加载物理量到左侧物理量框
    /// </summary>
    public void LoadQuantities()
    {
        foreach (var item in quantities)
            Destroy(item);
        quantities.Clear();

        foreach (var item in RecordManager.tempRecord.quantities)
        {
            var cell = Instantiate(_QuantitySelectCell, _Content.transform);
            quantities.Add(cell);
            var cellScript = cell.GetComponent<QuantitySelectCell>();
            var cellButton = cell.GetComponent<Button>();
            cellScript.Quantity = item;
            cellButton.onClick.AddListener(() => StringExpressionInput.text += cellScript.Quantity.Symbol);
        }
    }
}
