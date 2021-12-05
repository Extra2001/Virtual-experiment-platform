/************************************************************************************
    作者：张峻凡
    描述：系统对用户指出错误并给出评价流程
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 系统对用户指出错误并给出评价流程
/// </summary>
public class ProcessResultProcedure : ProcedureBase
{
    /// <summary>
    /// 进入流程
    /// </summary>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        RenderManager._SetTips("您可以返回并改正您的错误（如有）。");
        Main.m_UI.OpenUI<ProcessResult>();
        base.OnEnter(lastProcedure);
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<ProcessResult>();
        base.OnLeave(nextProcedure);
    }
}