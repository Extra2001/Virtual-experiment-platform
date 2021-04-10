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
    Button ContinueButton = null;
    Button QuitButton = null;
    Button RestartButton = null;
    Button SaveRecordButton = null;
    Button ReadRecordButton = null;

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
                ContinueButton = item;
                item.onClick.AddListener(() =>
                {
                    PauseManager.Instance.Continue();
                });
            }
            else if (item.gameObject.name.Equals("QuitButton"))
            {
                QuitButton = item;
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
                RestartButton = item;
                item.onClick.AddListener(() =>
                {
                    PauseManager.Instance.Continue();
                    GameManager.Instance.SwitchBackToStart();
                });
            }
            else if (item.gameObject.name.Equals("SaveRecordButton"))
            {
                SaveRecordButton = item;
                item.onClick.AddListener(() =>
                {
                    Main.m_UI.OpenTemporaryUI<SaveRecordUILogic>();
                });
                
            }
            else if (item.gameObject.name.Equals("ReadRecordButton"))
            {
                ReadRecordButton = item;
                item.onClick.AddListener(() =>
                {
                    Main.m_UI.OpenTemporaryUI<SettingsLogicTemporary>();
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
                if ((e as PauseEventHandler).NeedOpenPauseMenu)
                    Main.m_UI.OpenTemporaryUI<PauseUILogic>();
            }
            else if((e as PauseEventHandler).NeedOpenPauseMenu)
            {
                UIShowHideHelper.HideToUp(UIEntity);

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
        if (Main.m_Procedure.CurrentProcedure is StartProcedure)
            SaveRecordButton.interactable = false;
        else SaveRecordButton.interactable = true;
        UIShowHideHelper.ShowFromUp(UIEntity);
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public override void OnClose()
    {
        base.OnClose();
    }

    /// <summary>s
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
