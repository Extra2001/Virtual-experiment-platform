using HT.Framework;
using UnityEngine.UI;

public class NextButtonOnEnterExpression : HTBehaviour
{
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
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        ShowCancel = false,
                        Title = "错误",
                        Message = $"{message}，请检查输入。"
                    });
                }
            });
        });
    }
}
