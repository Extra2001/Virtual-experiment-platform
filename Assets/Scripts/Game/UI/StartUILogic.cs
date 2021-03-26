using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Start")]
public class StartUILogic : UILogicResident
{
    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();

        UIEntity.FindChildren("NewGameButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Main.m_Event.Throw<StartNewExpEventHandler>();
        });
        UIEntity.FindChildren("ExitButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Application.Quit();
        });
        bool showed = false;
        UIEntity.FindChildren("ContinueButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (showed)
            {
                showed = false;
                UIAPI.Instance.HideIndicator<Indicator3UILogic>();
            }
            else
            {
                showed = true;
                UIAPI.Instance.ShowIndicator<Indicator3UILogic>("E", "打开哈哈哈");
            }
        });
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public override void OnClose()
    {
        base.OnClose();
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
}
