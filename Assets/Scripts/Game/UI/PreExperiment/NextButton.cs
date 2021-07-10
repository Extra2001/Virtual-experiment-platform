/************************************************************************************
    作者：荆煦添
    描述：下一步按钮处理程序
*************************************************************************************/
using HT.Framework;
using System;
using UnityEngine.UI;

public class NextButton : HTBehaviour
{
    public HHHH nextProcedure;

    public Type[] types = { typeof(AddValueEventHandler), typeof(EnterExpressionEventHandler), typeof(PreviewConfirmEventHandler) };

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Main.m_Event.Throw(types[(int)nextProcedure]);
        });
    }

    public enum HHHH
    {
        EnterExpression,
        Preview,
        EnterClassroom
    }
}
