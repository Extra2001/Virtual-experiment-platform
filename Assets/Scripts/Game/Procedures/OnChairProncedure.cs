/************************************************************************************
    作者：张峻凡
    描述：选定位置，进入测量阶段
*************************************************************************************/
using UnityEngine;
using HT.Framework;

/// <summary>
/// 选定位置，进入测量阶段
/// </summary>
public class OnChairProcedure : ProcedureBase
{
    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        RenderManager._SetTips("鼠标中键调整视角。左键物体选定后，按Q和E切换平移、旋转。右键物体可扶正。");
        RenderManager.Instance.Show();
        var Position = NearChair.Instance.transform.position;
        Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<SitdownEventHandler>().Fill(Position.x, Position.y, Position.z));
        //人物可否移动
        GameManager.Instance.FPSable = true;
        //记录人物位置
        GameManager.Instance.PersonPosition = RecordManager.tempRecord.FPSPosition;
        GameManager.Instance.PersonRotation = RecordManager.tempRecord.FPSRotation;

        base.OnEnter(lastProcedure);
        //打开相应UI
        Main.m_UI.OpenUI<GameButtonUILogic>();

        Main.m_UI.OpenUI<BagControl>();
        (Main.m_UI.GetUI<BagControl>() as BagControl).Hide();
        //注册按键
        KeyboardManager.Instance.Register(KeyCode.T, () =>
        {
            if (Main.m_UI.GetOpenedUI<DatatableUILogic>() != null)
                UIAPI.Instance.HideDataTable();
            else
                UIAPI.Instance.ShowDataTable();
        });
        KeyboardManager.Instance.Register(KeyCode.B, () =>
        {
            if (Main.m_UI.GetOpenedUI<BagControl>() == null)
                Main.m_UI.OpenUI<BagControl>();
            else
                (Main.m_UI.GetUI<BagControl>() as BagControl).Hide();
        });
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        //关闭UI，取消注册按键
        RenderManager.Instance.Hide();
        GameManager.Instance.FPSable = false;
        base.OnLeave(nextProcedure);
        UIAPI.Instance.HideDataTable();
        (Main.m_UI.GetUI<BagControl>() as BagControl).Hide();
        Main.m_UI.CloseUI<GameButtonUILogic>();
        KeyboardManager.Instance.UnRegister(KeyCode.T);
        KeyboardManager.Instance.UnRegister(KeyCode.B);
    }

    /// <summary>
    /// 流程帧刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        //将当前人物位置存入存档
        RecordManager.tempRecord.FPSPosition = GameManager.Instance.PersonPosition.GetMyVector();
        RecordManager.tempRecord.FPSRotation = GameManager.Instance.PersonRotation.GetMyVector();
    }
}