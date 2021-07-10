/************************************************************************************
    作者：荆煦添
    描述：预览实验数据
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewExp : HTBehaviour
{
    public GameObject _QuantityPreviewCell;
    public GameObject _Content;
    public Image _Image;

    private List<GameObject> quantities = new List<GameObject>();

    private void Start()
    {
        LoadQuantities();
        Render();
    }
    /// <summary>
    /// 渲染合成量表达式图片
    /// </summary>
    public void Render()
    {
        var exp = CalcArgs.GetSymexpr(RecordManager.tempRecord.stringExpression).ToLaTeX();
        LatexEquationRender.Render(exp, x =>
        {
            _Image.FitHeight(x);
        });
    }
    /// <summary>
    /// 加载所有的物理量到列表
    /// </summary>
    public void LoadQuantities()
    {
        foreach (var item in quantities)
            Destroy(item);
        quantities.Clear();

        foreach (var item in RecordManager.tempRecord.quantities)
        {
            var cell = Instantiate(_QuantityPreviewCell, _Content.transform);
            quantities.Add(cell);
            var cellScript = cell.GetComponent<QuantityPreviewCell>();
            cellScript.SetQuantity(item);
        }
    }
}
