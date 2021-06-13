using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using UnityEngine.UI;
using System;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/GameButton")]
public class GameButtonUILogic : UILogicResident
{
    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();        

        UIEntity.FindChildren("PauseButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            PauseManager.Instance.Pause();
        });
        UIEntity.FindChildren("BagButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Main.m_UI.OpenTemporaryUI<BagControl>();
        });
        UIEntity.FindChildren("TableButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (Main.m_UI.GetOpenedUI<DatatableUILogic>() != null)
                UIAPI.Instance.HideDataTable();
            else
                UIAPI.Instance.ShowDataTable();
        });
        UIEntity.FindChildren("UncertaintyButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Main.m_Event.Throw<ProcessExplainEventHandler>();
        });
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        Main.m_Event.Subscribe<SelectInstrumentEventHandler>(ShowButtons);
    }

    /// <summary>
    /// 置顶UI
    /// </summary>
    public override void OnPlaceTop()
    {
        base.OnPlaceTop();
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public override void OnClose()
    {
        base.OnClose();
        Main.m_Event.Unsubscribe<SelectInstrumentEventHandler>(ShowButtons);
    }

    /// <summary>
    /// 销毁UI
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// UI逻辑刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    private void ShowButtons(EventHandlerBase handler)
    {
        InstrumentInfoModel model = (handler as SelectInstrumentEventHandler).ActiveInstrument;
        Debug.Log(model.instrumentType);
    }

}
