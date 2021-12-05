/************************************************************************************
    作者：张柯
    描述：预览实验数据流程
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 预览实验数据流程
/// </summary>
public class PreviewProcedure : ProcedureBase
{
    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        RenderManager._SetTips("进入实验室后，仍可点击左上角的选项，更改实验目标。");
        Main.m_UI.OpenUI<PreviewUILogic>();
        base.OnEnter(lastProcedure);
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<PreviewUILogic>();
        base.OnLeave(nextProcedure);
    }
}