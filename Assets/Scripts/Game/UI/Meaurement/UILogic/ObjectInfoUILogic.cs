/************************************************************************************
    作者：荆煦添
    描述：右键被测物体信息UI逻辑类
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 右键被测物体信息UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Measurment/ObjectInfoPanel")]
public class ObjectInfoUILogic : UILogicTemporary
{
    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        if (args.Length > 0)
        {
            var value = args[0] as ObjectValue;
            UIEntity.GetComponent<ObjectInfo>().Show(value);
        }
    }
}
