/************************************************************************************
    作者：张柯
    描述：离开数据生声明阶段，进入教室，本阶段选择测量所在的桌子
*************************************************************************************/
using UnityEngine;
using HT.Framework;

/// <summary>
/// 离开数据生声明阶段，进入教室，本阶段选择测量所在的桌子
/// </summary>
public class EnterClassroomProcedure : ProcedureBase
{
    bool showed = false;

    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        //此处指定位置为左上角，后续会开放至所有桌子
        RenderManager.Instance.Show();
        MainThread.Instance.DelayAndRun(1000, () =>
        {
            var Position = NearChair.Instance.transform.position;
            Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<Sitdown>().Fill(Position.x, Position.y, Position.z));
            GameManager.Instance.SwitchProcedure<OnChairProcedure>();
        });
        return;
        //GameManager.Instance.FPSable = true;
        //showed = false;
        //Main.m_UI.OpenTemporaryUI<DatatableUILogic>();
        //Main.m_UI.OpenResidentUI<GameButtonUILogic>();
        //KeyboardManager.Instance.Register(KeyCode.T, () =>
        //{
        //    if (showed)
        //        UIAPI.Instance.HideDataTable();
        //    else
        //        UIAPI.Instance.ShowDataTable();
        //    showed = !showed;
        //});
        //base.OnEnter(lastProcedure);
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        //关闭UI,取消注册按钮
        RenderManager.Instance.Hide();
        GameManager.Instance.FPSable = false;
        if (showed)
            UIAPI.Instance.HideDataTable();
        KeyboardManager.Instance.UnRegister(KeyCode.T);
        Main.m_UI.CloseUI<GameButtonUILogic>();
        base.OnLeave(nextProcedure);
    }
}