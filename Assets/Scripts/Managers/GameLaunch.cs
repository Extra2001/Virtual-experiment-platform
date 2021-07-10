/************************************************************************************
    作者：荆煦添
    描述：启动器，在程序启动时预加载资源和配置
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using System.Linq;

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
        Initializer.InitializeObjects();
        Initializer.InitializeExperiments();

        // 加载仪器
        foreach (var item in CommonTools.GetSubClassNames(typeof(InstrumentBase)).Where(x => !x.IsAbstract))
            Main.m_Entity.CreateEntity(item, entityName: item.Name, loadDoneAction: x => Main.m_Entity.HideEntity(x));
    }
    /// <summary>
    /// 启动服务程序
    /// </summary>
    private void LaunchServices()
    {
        Main.m_Event.Throw<ServiceStartedEventHandler>();
    }

    /// <summary>
    /// 启动单例模式的MonoBeheavior管理器
    /// </summary>
    private void LaunchManagers()
    {
        MainThread.Enable();

        GameManager.Enable();

        PauseManager.Enable();

        KeyboardManager.Enable();
    }
}
