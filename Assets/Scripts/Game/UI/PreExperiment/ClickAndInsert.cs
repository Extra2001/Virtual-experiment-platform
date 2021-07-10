/************************************************************************************
    作者：荆煦添
    描述：点击按钮插入公式
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class ClickAndInsert : HTBehaviour
{
    public EnterExpression enterExpressionInstance;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            enterExpressionInstance.StringExpressionInput.text += GetComponentInChildren<Text>().text;
        });
    }
}
