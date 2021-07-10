/************************************************************************************
    作者：荆煦添
    描述：暂停状态变更事件
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 暂停状态变更事件
/// </summary>
public class PauseEventHandler : EventHandlerBase
{
    public bool Paused { get; private set; } = false;
    public bool NeedOpenPauseMenu { get; private set; } = true;
    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public PauseEventHandler Fill(bool paused)
    {
        Paused = paused;
        return this;
    }
    public PauseEventHandler Fill(bool paused, bool needOpenPauseMenu)
    {
        Paused = paused;
        NeedOpenPauseMenu = needOpenPauseMenu;
        return this;
    }
    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {
        NeedOpenPauseMenu = true;
    }
}
