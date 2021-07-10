/************************************************************************************
    ���ߣ�������
    �����������ť���빫ʽ
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
