/************************************************************************************
    作者：荆煦添
    描述：输入合成量表达式UI逻辑类
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 输入合成量表达式UI逻辑类
/// </summary>
[UIResource(null, null, "UI/PreExperiment/EnterExpressionPanel")]
public class EnterExpressionUILogic : UILogicResident
{
	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        UIEntity.GetComponent<EnterExpression>()?.Show();
    }
}
