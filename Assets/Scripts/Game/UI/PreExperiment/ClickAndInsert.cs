using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickAndInsert : HTBehaviour
{
    public EnterExpression enterExpressionInstance;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            enterExpressionInstance.StringExpressionInput.text += GetComponentInChildren<Text>().text;
        });
    }
}
