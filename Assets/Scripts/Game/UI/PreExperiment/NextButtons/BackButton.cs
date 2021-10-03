/************************************************************************************
    作者：荆煦添
    描述：上一步按钮处理程序
*************************************************************************************/
using HT.Framework;
using System;
using UnityEngine.UI;

public class BackButton : HTBehaviour
{
    public bool inverse = false;
    public Type UILogic = null;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (inverse)
                Hide();
            else
                GameManager.Instance.SwitchBackProcedure();
        });
    }

    private void HideEntity()
    {
        var ui = Main.m_UI.GetOpenedUI(UILogic);
        UIShowHideHelper.HideToButtom(ui.UIEntity);
        Invoke(nameof(Close), 0.3f);
    }

    private void Close()
    {
        Main.m_UI.CloseUI(UILogic);
    }

    private void Hide()
    {
        if (UILogic.Name.Equals(typeof(AddValueUILogic).Name))
        {
            if (ValueValidator.ValidateQuantities(RecordManager.tempRecord.quantities))
                HideEntity();
        }
        else if (UILogic.Name.Equals(typeof(EnterExpressionUILogic).Name))
        {
            Main.m_UI.GetOpenedUI<EnterExpressionUILogic>().UIEntity.GetComponent<EnterExpression>().Validate((res, message) =>
            {
                if (res)
                {
                    HideEntity();
                }
                else
                {
                    UIAPI.Instance.ShowModel(new ModelDialogModel()
                    {
                        ShowCancel = false,
                        Title = new BindableString("错误"),
                        Message = new BindableString($"{message}，请检查输入。")
                    });
                }
            });
        }
        else
        {
            HideEntity();
        }
    }
}
