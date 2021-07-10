/************************************************************************************
    作者：张峻凡
    描述：合成量数据处理的UI逻辑类
*************************************************************************************/
using HT.Framework;

/// <summary>
/// 合成量数据处理的UI逻辑类
/// </summary>
[UIResource(null, null, "UI/DataProcess/ComplexDataProcess")]
public class ComplexData : UILogicResident
{
	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        UIEntity.GetComponent<DealComplexDataInput>().StartShow();
    }
}
