/************************************************************************************
    作者：张峻凡
    描述：直接测量量数据处理的UI逻辑类
*************************************************************************************/

using HT.Framework;
/// <summary>
/// 直接测量量数据处理的UI逻辑类
/// </summary>
[UIResource(null, null, "UI/DataProcess/MeasuredDataProcess")]
public class MeasuredDataProcess : UILogicResident
{
	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {       
        base.OnOpen(args);
        QuantityModel quantity = (QuantityModel)args[0];
        UIEntity.GetComponent<MeasuredProcessController>().Show(quantity);
        //UIEntity.GetComponentInChildren<UncertaintyInput>(true).Show(quantity);
        //UIEntity.GetComponentInChildren<DealMeasuredDataInput>(true).Show(quantity);
    }
}
