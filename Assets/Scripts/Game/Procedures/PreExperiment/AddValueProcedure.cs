/************************************************************************************
    作者：张柯
    描述：添加物理量流程
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 进入添加物理量流程
/// </summary>
public class AddValueProcedure : ProcedureBase
{
    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RenderManager._SetTips("请添加至少一个直接测量的物理量。");
#endif
        Main.m_UI.OpenUI<AddValueUILogic>();
        base.OnEnter(lastProcedure);
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<AddValueUILogic>();
        base.OnLeave(nextProcedure);
    }
}