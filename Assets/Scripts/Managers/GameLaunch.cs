/************************************************************************************
    作者：荆煦添
    描述：启动器，在程序启动时预加载资源和配置
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameLaunch : MonoBehaviour
{
    public static GameLaunch Instance;

    public LoadingScreenManager2 InitLoading;
    public LoadingScreenManager LoadingScreen;
    public Image GeneralLoadingScreen;

    public void ShowGeneralLoadingScreen()
    {
        GeneralLoadingScreen.gameObject.SetActive(true);
    }
    public void HideGeneralLoadingScreen()
    {
        GeneralLoadingScreen.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
        InitLoading.gameObject.SetActive(true);
        LoadingScreen.gameObject.SetActive(true);

        ProcessManager.StartService();

        UIAPIInitializer.Current.Initialize();

        GetComponent<RenderManager>().Hide();

        LaunchManagers();

        PreLoadingAssets();
    }
    /// <summary>
    /// 预加载资产
    /// </summary>
    private void PreLoadingAssets()
    {
        Initializer.InitializeObjects();
        Initializer.InitializeExperiments();
        StartCoroutine(Initializer.PreLoadImages());
        // 加载仪器
        foreach (var item in CommonTools.GetSubClassNames(typeof(InstrumentBase))
            .Where(x => !x.IsAbstract).OrderBy(x => x.CreateInstrumentInstance().ID))
            Main.m_Entity.CreateEntity(item, entityName: item.Name, loadDoneAction: x => Main.m_Entity.HideEntity(x));
        
    }
    /// <summary>
    /// 启动单例模式的MonoBeheavior管理器
    /// </summary>
    private void LaunchManagers()
    {
        WisdomTree.Common.Function.Communication.Init(() => RecordManager.UpdateRecordInfos(null));

        MainThread.Enable();

        GameManager.Enable();

        KeyboardManager.Enable();

        PauseManager.Enable();
    }
    private void OnDestroy()
    {
        ProcessManager.StopService();
    }
}
