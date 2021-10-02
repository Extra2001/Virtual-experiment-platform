using HT.Framework;
using UnityEngine.UI;

public class NextButtonOnEnterExpression : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Main.m_UI.GetOpenedUI<EnterExpressionUILogic>().UIEntity.GetComponent<EnterExpression>().Validate(res =>
            {
                if (res)
                {
                    GameManager.Instance.SwitchProcedure<PreviewProcedure>();
                }
                else
                {
                    UIAPI.Instance.ShowModel(new ModelDialogModel()
                    {
                        ShowCancel = false,
                        Title = new BindableString("错误"),
                        Message = new BindableString("渲染表达式错误，请检查输入。")
                    });
                }
            });
        });
    }
}
