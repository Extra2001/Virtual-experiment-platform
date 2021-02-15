using HT.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        UIAPIInitializer.Current.Initialize();

        PreLoadingAssets();

        LaunchServices();

        LaunchManagers();
    }

    /// <summary>
    /// 预加载资产
    /// </summary>
    private void PreLoadingAssets()
    {
        
    }

    private void LaunchServices()
    {
        // 启动服务程序

        Main.m_Event.Throw<ServiceStartedEventHandler>();
    }

    /// <summary>
    /// 启动MonoBeheavior管理器
    /// </summary>
    private void LaunchManagers()
    {
        MainThread.Enable();

        GameManager.Current.LoadStartScreen();

        PauseManager.Enable();
    }
}
