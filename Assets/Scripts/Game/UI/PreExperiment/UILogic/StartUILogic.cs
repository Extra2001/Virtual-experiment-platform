using UnityEngine;
using HT.Framework;
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
        UIEntity.FindChildren("ContinueButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            UIAPI.Instance.ShowAndHideLoading(1000);
            MainThread.Instance.DelayAndRun(500, () =>
            {
                GameManager.Instance.ContinueExp();
            });
        });
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        LoadingScreenManager2.Instance.HideLoadingScreen();
        if (GameManager.Instance.CanContinue)
            UIEntity.FindChildren("ContinueButton").GetComponent<Button>().interactable = true;
        else
            UIEntity.FindChildren("ContinueButton").GetComponent<Button>().interactable = false;

        base.OnOpen(args);
    }
}
