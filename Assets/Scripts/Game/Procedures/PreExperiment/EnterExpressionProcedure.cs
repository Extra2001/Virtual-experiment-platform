/************************************************************************************
    作者：张柯
    描述：输入合成量表达式流程
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 进入输入合成量表达式流程
/// </summary>
public class EnterExpressionProcedure : ProcedureBase
{
    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        RenderManager._SetTips("请输入您测量目标的合成表达式。点击左侧可插入符号。");
        Main.m_UI.OpenUI<EnterExpressionUILogic>();
        base.OnEnter(lastProcedure);
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<EnterExpressionUILogic>();
        base.OnLeave(nextProcedure);
    }
}