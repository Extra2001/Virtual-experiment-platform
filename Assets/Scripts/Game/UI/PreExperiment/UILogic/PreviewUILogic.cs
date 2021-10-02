/************************************************************************************
    作者：荆煦添
    描述：预览物理量UI逻辑类
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 预览物理量UI逻辑类
/// </summary>
[UIResource(null, null, "UI/PreExperiment/PreviewPanel")]
public class PreviewUILogic : UILogicResident
{
	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        UIEntity.GetComponent<PreviewExp>()?.Show();
    }
}
