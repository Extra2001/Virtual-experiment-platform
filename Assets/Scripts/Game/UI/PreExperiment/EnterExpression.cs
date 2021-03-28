using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class EnterExpression : HTBehaviour
{
    public GameObject _Content;
    public GameObject _QuantitySelectCell;
    public InputField StringExpressionInput;
    public InputField LatexExpressionInput;
    public Button StringExpressionCheckButton;
    public Button LatexExpressionCheckButton;
    public SegmentedControl SegmentedControl;

    private List<GameObject> quantities = new List<GameObject>();
    private InputField ExpressionInput;
    private Button ExpressionCheckButton;

    // Start is called before the first frame update
    void Start()
    {
        SegmentedControl.selectedSegmentIndex = (int)RecordManager.tempRecord.expressionKind;
        LoadQuantities();
        SyncExpression();
        SegmentedControl.onValueChanged.AddListener(value =>
        {
            RecordManager.tempRecord.expressionKind = (Expression)value;
            SyncExpression();
        });
    }

    private void SyncExpression()
    {
        if (RecordManager.tempRecord.expressionKind == Expression.String)
        {
            StringExpressionCheckButton.gameObject.transform.parent.gameObject.SetActive(true);
            LatexExpressionCheckButton.gameObject.transform.parent.gameObject.SetActive(false);
            ExpressionInput = StringExpressionInput;
            ExpressionCheckButton = StringExpressionCheckButton;
        }
        else if (RecordManager.tempRecord.expressionKind == Expression.Latex)
        {
            LatexExpressionCheckButton.gameObject.transform.parent.gameObject.SetActive(true);
            StringExpressionCheckButton.gameObject.transform.parent.gameObject.SetActive(false);
            ExpressionInput = LatexExpressionInput;
            ExpressionCheckButton = LatexExpressionCheckButton;
        }

        LatexExpressionInput.text = RecordManager.tempRecord.latexExpression;
        StringExpressionInput.text = RecordManager.tempRecord.stringExpression;
    }

    // Update is called once per frame
    void Update()
    {
        SaveExpression();
    }

    private void SaveExpression()
    {
        RecordManager.tempRecord.latexExpression = LatexExpressionInput.text;
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
            cellButton.onClick.AddListener(() => ExpressionInput.text += cellScript.Quantity.Symbol);
        }
    }
}
