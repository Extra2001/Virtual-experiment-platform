/************************************************************************************
    作者：张峻凡
    描述：面板按钮UI逻辑类
*************************************************************************************/
using System.Collections.Generic;
using HT.Framework;
using UnityEngine.UI;
using System;
/// <summary>
/// 面板按钮UI逻辑类
/// </summary>
[UIResource(null, null, "UI/GameButton")]
public class GameButtonUILogic : UILogicResident
{
    public List<GameButtonItem> gameButtonItems = new List<GameButtonItem>();

    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();

        foreach (var item in UIEntity.GetComponentsInChildren<Button>(true))
        {
            var it = new GameButtonItem()
            {
                GameObject = item
            };
            item.onClick.AddListener(() =>
            {
                foreach (var itemm in it.OnClick)
                    itemm?.Invoke();
            });
            gameButtonItems.Add(it);
        }

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
            GameManager.Instance.SwitchProcedure<ProcessExplainProcedure>();
        });
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        Main.m_Event.Subscribe<SelectInstrumentEventHandler>(ShowButtons);
        if (GameManager.Instance.CurrentInstrument != null)
            ShowButtons(GameManager.Instance.CurrentInstrument);
        else
        {
            string[] names = new string[] { "PauseButton", "BagButton", "TableButton", "UncertaintyButton", "OptionsButton" };

            gameButtonItems.ForEach(x =>
            {
                x.OnClick.Clear();
                x.OnTap.Clear();
                if (!names.Contains(x.GameObject.name)) x.GameObject.gameObject.SetActive(false);
            });
        }
        //复现存档仪器被测物体位置等信息
        CreateObject.CreateRecord();
        CreateInstrument.CreateRecord();
        UIAPI.Instance.HideLoading();
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
    /// 调用仪器类接口决定显示哪些按钮
    /// </summary>
    /// <param name="handler"></param>
    private void ShowButtons(EventHandlerBase handler)
    {
        var instrument = (handler as SelectInstrumentEventHandler).ActiveInstrument;
        ShowButtons(instrument);
    }
    private void ShowButtons(InstrumentBase instrument)
    {
        string[] names = new string[] { "PauseButton", "BagButton", "TableButton", "UncertaintyButton" };

        gameButtonItems.ForEach(x =>
        {
            x.OnClick.Clear();
            x.OnTap.Clear();
            if (!names.Contains(x.GameObject.name)) x.GameObject.gameObject.SetActive(false);
        });

        instrument.ShowGameButton(gameButtonItems);

        foreach (var item in gameButtonItems)
            if (item.OnClick.Count != 0 || item.OnTap.Count != 0)
                item.GameObject.gameObject.SetActive(true);
    }
}

public class GameButtonItem
{
    public Button GameObject { get; set; }

    public List<Action> OnClick { get; set; } = new List<Action>();

    public List<Action> OnTap { get; set; } = new List<Action>();
}