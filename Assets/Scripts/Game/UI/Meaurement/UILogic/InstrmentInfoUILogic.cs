/************************************************************************************
    作者：荆煦添
    描述：右键仪器信息UI逻辑类
*************************************************************************************/
using HT.Framework;
using System;
/// <summary>
/// 右键仪器信息UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Measurment/InstrumentInfoPanel")]
public class InstrmentInfoUILogic : UILogicTemporary
{
	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        if (args.Length > 0)
        {
            Type instument = (Type)args[0];
            UIEntity.GetComponent<InstrumentInfo>().ShowInstrument(instument);
        }
    }
}
