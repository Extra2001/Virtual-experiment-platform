using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Threading.Tasks;

public class EnterExpression : HTBehaviour
{
    public GameObject _Content;
    public GameObject _QuantitySelectCell;
    public InputField StringExpressionInput;
    public Image image;
    //public Button RenderButton;

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

    public void LoadSprite(Sprite sprite)
    {
        image.FitHeight(sprite);
    }

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

    public void Validate()
    {
        var text = StringExpressionInput.text;
        LatexEquationRender.Render(CalcArgs.GetSymexpr(text).ToLaTeX());
    }

    public void SyncExpression()
    {
        StringExpressionInput.text = RecordManager.tempRecord.stringExpression;
        Render();
    }


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
