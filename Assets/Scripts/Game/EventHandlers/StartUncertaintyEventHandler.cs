/************************************************************************************
    作者：张峻凡
    描述：用户开始处理数据事件
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 用户开始处理数据事件
/// </summary>
public class StartUncertaintyEventHandler : EventHandlerBase
{
    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public StartUncertaintyEventHandler Fill()
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
