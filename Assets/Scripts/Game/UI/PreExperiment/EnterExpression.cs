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
    /// ��Ⱦ�ص�����ͼ��
    /// </summary>
    /// <param name="sprite"></param>
    public void LoadSprite(Sprite sprite)
    {
        image.FitHeight(sprite);
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
    /// ͬ�����ʽ���浵
    /// </summary>
    public void SyncExpression()
    {
        StringExpressionInput.text = RecordManager.tempRecord.stringExpression;
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
            cellButton.onClick.AddListener(() => StringExpressionInput.text += cellScript.Quantity.Symbol);
        }
    }
}
