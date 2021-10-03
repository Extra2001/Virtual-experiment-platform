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
            Main.m_UI.GetOpenedUI<EnterExpressionUILogic>().UIEntity.GetComponent<EnterExpression>().Validate((res, message) =>
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
                        Message = new BindableString($"{message}，请检查输入。")
                    });
                }
            });
        });
    }
}
