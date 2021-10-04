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
    public bool NeedOpenPauseMenu { get; private set; } = false;

    public PauseEventHandler Fill(bool paused, bool needOpenPauseMenu)
    {
        Paused = paused;
        NeedOpenPauseMenu = needOpenPauseMenu;
        return this;
    }

    public override void Reset()
    {
        NeedOpenPauseMenu = false;
    }
}
