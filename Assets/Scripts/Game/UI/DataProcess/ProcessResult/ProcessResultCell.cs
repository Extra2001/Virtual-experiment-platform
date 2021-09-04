using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessResultCell : HTBehaviour
{
    public GameObject ExpressionPanel;
    public GameObject FormulaControllerPanel;
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
                    ExpressionPanel.FindChildren("ExpressionImage").GetComponent<Image>().FitHeight(res);
                    //ExpressionPanel.FindChildren("ExpressionImage").GetComponent<Image>().sprite = res;
                });
        }
        if (userExpression == null)
            FormulaControllerPanel.SetActive(false);
        else
        {
            FormulaControllerPanel.SetActive(true);
            FormulaControllerPanel.GetComponentInChildren<FormulaController>(true).LoadFormula(userExpression);
        }
    }
}
