/************************************************************************************
    作者：张柯
    描述：起始流程
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 起始流程
/// </summary>
public class StartProcedure : ProcedureBase
{
    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RenderManager._SetTips("点击新实验，开始一个实验。");
#endif
        Main.m_UI.OpenUI<StartUILogic>();
        base.OnEnter(lastProcedure);
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<StartUILogic>();
        base.OnLeave(nextProcedure);
    }
}