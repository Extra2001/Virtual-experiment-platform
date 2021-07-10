/************************************************************************************
    作者：荆煦添
    描述：工作存档变更事件
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 工作存档变更事件
/// </summary>
public class ChangeRecordEventHandler : EventHandlerBase
{
    public int currentRecordId;
    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public ChangeRecordEventHandler Fill(int id)
    {
        currentRecordId = id;
        return this;
    }

    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {
        
    }
}
