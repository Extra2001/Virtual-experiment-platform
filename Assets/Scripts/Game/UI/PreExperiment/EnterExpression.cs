/************************************************************************************
    ���ߣ�������
    ����������ϳ������ʽ�������
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Events;
using System.Linq;
using System;

public class EnterExpression : HTBehaviour
{
    public GameObject _Content;
    public GameObject _QuantitySelectCell;
    public InputField StringExpressionInput;
    public Image image;
    public GameObject Loading;
    private List<GameObject> quantities = new List<GameObject>();

    private void Start()
    {
        StringExpressionInput.onValueChanged.AddListener(_ =>
        {
            RecordManager.tempRecord.stringExpression = ExpressionToCalc(StringExpressionInput.text);
            Render();
        });
    }

    public void Show()
    {
        LoadQuantities();
        SyncExpression();
    }

    /// <summary>
    /// ��Ⱦ�ص�����ͼ��
    /// </summary>
    /// <param name="sprite"></param>
    public void LoadSprite(Sprite sprite)
    {
        image.FitImage(sprite, 270, 40);
    }
    /// <summary>
    /// ��Ⱦ���ʽ
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
    /// �첽У����ʽ
    /// </summary>
    /// <param name="error"></param>
    public void Validate(UnityAction<bool, string> error)
    {
        var text = StringExpressionInput.text;
        Loading.SetActive(true);
        try
        {
            var sym = CalcArgs.GetSymexpr(text);
            var list = new List<string>();
            GetAllFactors(sym, list);
            foreach(var item in RecordManager.tempRecord.quantities)
            {
                if(item.processMethod == 1)
                {
                    if (list.Contains("k_" + item.Symbol) || list.Contains("��" + item.Symbol))
                    {
                        Loading.SetActive(false);
                        error?.Invoke(false, $"ֱ�Ӽ����������{item.Name}����ʹ�ñ仯����仯��");
                        return;
                    }
                }
                else if (item.processMethod == 2)
                {
                    if (list.Contains("k_" + item.Symbol) || list.Contains(item.Symbol))
                    {
                        Loading.SetActive(false);
                        error?.Invoke(false, $"�������������{item.Name}����ֱ��ʹ�������ƽ��ֵ��仯��");
                        return;
                    }
                }
                else if (item.processMethod == 3 || item.processMethod == 4)
                {
                    if (list.Contains("k_" + item.Symbol) || list.Contains("��" + item.Symbol))
                    {
                        Loading.SetActive(false);
                        error?.Invoke(false, $"������{item.Name}����ֱ��ʹ�������ƽ��ֵ��仯��");
                        return;
                    }
                }
            }
            LatexEquationRender.Render(sym.ToLaTeX(),
                action: _ =>
                {
                    Loading.SetActive(false);
                    var (res, message) = StaticMethods.ValidExpression(ExpressionToCalc(text));
                    error?.Invoke(res, message);
                },
                errorHandler: () =>
                {
                    Loading.SetActive(false);
                    error?.Invoke(false, "���ʽ��Ⱦʧ��");
                });
        }
        catch
        {
            Loading.SetActive(false);
            error?.Invoke(false, "���ʽ��ʽ����ȷ");
        }
    }
    /// <summary>
    /// ͬ�����ʽ���浵
    /// </summary>
    public void SyncExpression()
    {
        StringExpressionInput.text = ExpressionToShow(RecordManager.tempRecord.stringExpression);
        Render();
    }
    /// <summary>
    /// �����������������������
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
            cellButton.onClick.AddListener(() =>
            {
                if (cellScript.Quantity.processMethod == 1)
                    StringExpressionInput.text += cellScript.Quantity.Symbol;
                else if (cellScript.Quantity.processMethod == 2)
                    StringExpressionInput.text += "��" + cellScript.Quantity.Symbol;
                else if (cellScript.Quantity.processMethod == 3 || cellScript.Quantity.processMethod == 4)
                    StringExpressionInput.text += "k_" + cellScript.Quantity.Symbol;
            });
        }
    }

    public static string ExpressionToCalc(string expr)
    {
        expr = expr.Replace("��", "").Replace("k_", "");
        return expr;
    }

    public static void GetAllFactors(MathNet.Symbolics.SymbolicExpression sym, List<string> list)
    {
        if (sym.Factors().Count() == 1)
        {
            try { var tmp = sym.RealNumberValue; }
            catch
            {
                if (sym.Expression.IsPower)
                {
                    var hh = sym.Expression as MathNet.Symbolics.Expression.Power;
                    if (hh.Item1.IsIdentifier)
                    {
                        if (!list.Contains((hh.Item1 as MathNet.Symbolics.Expression.Identifier).Item.Item.ToString()))
                            list.Add((hh.Item1 as MathNet.Symbolics.Expression.Identifier).Item.Item.ToString());
                    }
                    else
                    {
                        var sym1 = new MathNet.Symbolics.SymbolicExpression(hh.Item1);
                        GetAllFactors(sym1, list);
                    }
                    if (hh.Item2.IsIdentifier)
                    {
                        if (!list.Contains((hh.Item2 as MathNet.Symbolics.Expression.Identifier).Item.Item.ToString()))
                            list.Add((hh.Item2 as MathNet.Symbolics.Expression.Identifier).Item.Item.ToString());
                    }
                    else
                    {
                        var sym1 = new MathNet.Symbolics.SymbolicExpression(hh.Item2);
                        GetAllFactors(sym1, list);
                    }
                }
                else if (sym.Expression.IsFunction)
                {
                    var hh = sym.Expression as MathNet.Symbolics.Expression.Function;
                    if (hh.Item2.IsIdentifier)
                    {
                        if (!list.Contains((hh.Item2 as MathNet.Symbolics.Expression.Identifier).Item.Item.ToString()))
                            list.Add((hh.Item2 as MathNet.Symbolics.Expression.Identifier).Item.Item.ToString());
                    }
                    else
                    {
                        var sym1 = new MathNet.Symbolics.SymbolicExpression(hh.Item2);
                        GetAllFactors(sym1, list);
                    }
                }
                else if (!list.Contains(sym.ToString()))
                    list.Add(sym.ToString());
            }
        }
        else
            foreach (var item in sym.Factors())
                GetAllFactors(item, list);
    }

    public static string ExpressionToShow(string expr)
    {
        try
        {
            var sym = CalcArgs.GetSymexpr(expr);
            var factors = new List<string>();
            GetAllFactors(sym, factors);
            factors = factors.OrderByDescending(x => x.Length).ToList();
            Dictionary<string, string> replacement = new Dictionary<string, string>();
            foreach (var item in factors)
            {
                string guid = Guid.NewGuid().ToString();
                replacement.Add(guid, item);
                expr = expr.Replace(item, guid);
            }
            foreach (var item in RecordManager.tempRecord.quantities)
            {
                var replace = replacement.Where(x => x.Value.Equals(item.Symbol));
                if (replace.Any())
                {
                    var tmp = replace.First();
                    if (item.processMethod == 1)
                        expr = expr.Replace(tmp.Key, item.Symbol);
                    else if (item.processMethod == 2)
                        expr = expr.Replace(tmp.Key, "��" + item.Symbol);
                    else if (item.processMethod == 3 || item.processMethod == 4)
                        expr = expr.Replace(tmp.Key, "k_" + item.Symbol);
                }
            }
        }
        catch { }
        return expr;
    }
}
