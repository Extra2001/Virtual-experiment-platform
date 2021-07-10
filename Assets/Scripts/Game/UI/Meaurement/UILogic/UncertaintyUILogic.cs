/************************************************************************************
    作者：张峻凡
    描述：计算不确定度UI逻辑类
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 计算不确定度UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Measurment/DataTable/UncertaintyPanel")]
public class UncertaintyUILogic : UILogicResident
{
	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        QuantityModel quantity = (QuantityModel)args[0];
        UIEntity.GetComponent<UncertaintyInput>().Show(quantity);
    }
}
