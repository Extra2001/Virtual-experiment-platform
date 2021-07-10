/************************************************************************************
    作者：张峻凡
    描述：显示数据处理结果的UI逻辑类
*************************************************************************************/

using HT.Framework;
/// <summary>
/// 显示数据处理结果的UI逻辑类
/// </summary>
[UIResource(null, null, "UI/DataProcess/ProcessResult")]
public class ProcessResult : UILogicResident
{
	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        UIEntity.GetComponent<DealProcessResult>().Show();
        base.OnOpen(args);
    }
}
