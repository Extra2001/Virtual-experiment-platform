/************************************************************************************
    作者：张峻凡
    描述：选择实验事件
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 选择实验事件
/// </summary>
public class ChooseExpEventHandler : EventHandlerBase
{
    public int expId;
    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public ChooseExpEventHandler Fill(int expId)
    {
        this.expId = expId;

        return this;
    }

    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {
        
    }
}
