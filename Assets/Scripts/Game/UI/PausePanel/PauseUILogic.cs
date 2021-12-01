/************************************************************************************
    作者：荆煦添
    描述：暂停面板UI逻辑类
*************************************************************************************/
using UnityEngine;
using HT.Framework;
using UnityEngine.UI;
/// <summary>
/// 暂停面板UI逻辑类
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
                    PauseManager.Instance.Continue(true);
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
                    PauseManager.Instance.Continue(true);
                    GameManager.Instance.SwitchBackToStart();
                });
            }
            else if (item.gameObject.name.Equals("SaveRecordButton"))
            {
                SaveRecordButton = item;
                item.onClick.AddListener(() =>
                {
                    Main.m_UI.OpenUI<SaveRecordUILogic>();
                });

            }
            else if (item.gameObject.name.Equals("ReadRecordButton"))
            {
                ReadRecordButton = item;
                item.onClick.AddListener(() =>
                {
                    Main.m_UI.OpenUI<SettingsLogicTemporary>();
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
                    Main.m_UI.OpenUI<PauseUILogic>();
            }
            else if ((e as PauseEventHandler).NeedOpenPauseMenu)
            {
                UIShowHideHelper.HideToUp(UIEntity);
                MainThread.Instance.DelayAndRun(300, Main.m_UI.CloseUI<PauseUILogic>);
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
}
