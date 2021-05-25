using HT.Framework;
using System.Collections;
using System.Collections.Generic;
using Dummiesman;
using System.IO;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
    public LoadingScreenManager2 InitLoading;
    public LoadingScreenManager LoadingScreen;

    private void Awake()
    {
        InitLoading.gameObject.SetActive(true);
        LoadingScreen.gameObject.SetActive(true);

        UIAPIInitializer.Current.Initialize();

        PreLoadingAssets();

        LaunchServices();

        LaunchManagers();

        GetComponent<RenderManager>().Hide();
    }

    /// <summary>
    /// 预加载资产
    /// </summary>
    private void PreLoadingAssets()
    {
        // 加载仪器
        foreach (var item in CommonTools.GetSubClassNames(typeof(InstrumentBase)))
            Main.m_Entity.CreateEntity(item, entityName: item.Name, loadDoneAction: x => Main.m_Entity.HideEntity(x));
    }

    private void LaunchServices()
    {
        // 启动服务程序

        //ProcessManager.StartService();

        Main.m_Event.Throw<ServiceStartedEventHandler>();
    }

    /// <summary>
    /// 启动MonoBeheavior管理器
    /// </summary>
    private void LaunchManagers()
    {
        MainThread.Enable();

        GameManager.Enable();

        PauseManager.Enable();

        KeyboardManager.Enable();
    }

    private void OnDestroy()
    {
        // ProcessManager.StopService();
    }
}
