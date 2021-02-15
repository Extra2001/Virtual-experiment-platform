using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/PauseMenu")]
public class PauseUILogic : UILogicTemporary
{
    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();

        SubscribeEvent();
        AddInnerLogic();
    }

    /// <summary>
    /// 添加UI内部点击逻辑
    /// </summary>
    private void AddInnerLogic()
    {
        foreach (var item in UIEntity.GetComponentsInChildren<Button>(true))
        {
            if (item.gameObject.name.Equals("ContinueButton"))
            {
                item.onClick.AddListener(() =>
                {
                    PauseManager.Instance.Continue();
                });
            }
            else if (item.gameObject.name.Equals("QuitButton"))
            {
                item.onClick.AddListener(() =>
                {
                    UIAPI.Instance.ShowModel(new ModelDialogModel()
                    {
                        Title = new BindableString("提示"),
                        Message = new BindableString("您确定要退出吗？"),
                        ConfirmAction = () =>
                        {
                            Log.Info("程序已退出");
                            Application.Quit();
                        }
                    });
                });
            }
            else if (item.gameObject.name.Equals("RestartButton"))
            {
                item.onClick.AddListener(() =>
                {
                    PauseManager.Instance.Continue();
                    Main.m_Procedure.SwitchProcedure<StartProcedure>();
                });
            }
        }
    }

    /// <summary>
    /// 订阅暂停事件
    /// </summary>
    private void SubscribeEvent()
    {
        Main.m_Event.Subscribe<PauseEventHandler>((sender, e) =>
        {
            if ((e as PauseEventHandler).Paused)
            {
                Main.m_UI.OpenTemporaryUI<PauseUILogic>();
            }
            else
            {
                UIShowHideHelper.HideUpToDown(UIEntity);

                Task.Delay(300).ContinueWith(_ =>
                {
                    MainThread.Instance.Run(() =>
                    {
                        Main.m_UI.CloseUI<PauseUILogic>();
                    });
                });
            }
        });
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);

        UIShowHideHelper.ShowUpToDown(UIEntity);
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
