/************************************************************************************
    作者：张峻凡
    描述：添加物理量事件
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 添加物理量事件
/// </summary>
public class AddValueEventHandler : EventHandlerBase
{
    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public AddValueEventHandler Fill()
    {
        return this;
    }

    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {
        
    }
}
