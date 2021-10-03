using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessResultCell : HTBehaviour
{
    public GameObject ExpressionPanel;
    public Image Expression;
    public GameObject FormulaControllerPanel;
    public FormulaController Formula;
    public Text Title;
    public Text Detail;

    public void ShowData(string title, string message = null, string rightExpression = null, List<FormulaNode> userExpression = null)
    {
        Title.text = title.Trim();
        if (string.IsNullOrEmpty(message))
            Detail.gameObject.SetActive(false);
        else
        {
            Detail.gameObject.SetActive(true);
            Detail.text = message;
        }
        if (rightExpression == null)
            ExpressionPanel.SetActive(false);
        else
        {
            ExpressionPanel.SetActive(true);
            LatexEquationRender.Render(rightExpression, res =>
                {
                    Expression.FitImage(res, 220, 50);
                });
        }
        if (userExpression == null)
            FormulaControllerPanel.SetActive(false);
        else
        {
            FormulaControllerPanel.SetActive(true);
            Formula.LoadFormula(userExpression);
            Formula.interactable = false;
        }
    }
}
