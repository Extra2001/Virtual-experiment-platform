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

    // Start is called before the first frame update
    void Start()
    {
        LoadQuantities();
        SyncExpression();
        StringExpressionInput.onValueChanged.AddListener(_ => Render());
        //RenderButton.onClick.AddListener(Render);
    }

    // Update is called once per frame
    void Update()
    {
        SaveExpression();
    }

    public void LoadSprite(Sprite sprite)
    {
        image.sprite = sprite;
        var hh = image.gameObject.rectTransform().sizeDelta;
        hh.x = (float)sprite.texture.width / sprite.texture.height * hh.y;
        image.gameObject.rectTransform().sizeDelta = hh;
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

    private void SaveExpression()
    {
        RecordManager.tempRecord.stringExpression = StringExpressionInput.text;
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
