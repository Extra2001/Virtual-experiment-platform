/************************************************************************************
    作者：张峻凡
    描述：数据处理前的说明流程
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 数据处理前的说明流程
/// </summary>
public class ProcessExplainProcedure : ProcedureBase
{
    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        RenderManager._SetTips("");
        base.OnEnter(lastProcedure);
        Main.m_UI.OpenUI<ProcessExplain>();
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        base.OnLeave(nextProcedure);
        Main.m_UI.CloseUI<ProcessExplain>();
    }
}