/************************************************************************************
    作者：荆煦添
    描述：UI公共API
*************************************************************************************/
using HT.Framework;
using System;

public class UIAPI : SingletonBehaviorManager<UIAPI>
{
    public LoadingScreenManager LoadingScreenManager;

    public void ShowLoading()
    {
        LoadingScreenManager.RevealLoadingScreen();
    }

    public void ShowAndHideLoading(int delay)
    {
        ShowLoading();
        Invoke(nameof(HideLoading), delay / 1000f);
        //MainThread.Instance.DelayAndRun(delay, HideLoading);
    }

    public void HideLoading()
    {
        LoadingScreenManager.HideLoadingScreen();
    }

    public void ShowDataTable()
    {
        Main.m_UI.OpenResidentUI<DatatableUILogic>();
        Main.m_UI.GetUI<DatatableUILogic>().Show();
    }

    public void HideDataTable()
    {
        Main.m_UI.GetUI<DatatableUILogic>().Hide();
        MainThread.Instance.DelayAndRun(300, Main.m_UI.CloseUI<DatatableUILogic>);
    }

    public void ShowInstrumentInfo<T>() where T : InstrumentBase
    {
        Main.m_UI.OpenTemporaryUI<InstrmentInfoUILogic>(typeof(T));
    }

    public void ShowInstrumentInfo(Type instrument)
    {
        Main.m_UI.OpenTemporaryUI<InstrmentInfoUILogic>(instrument);
    }

    public void ShowModel(ModelDialogModel model)
    {
        Main.m_UI.OpenTemporaryUI<ModelUILogic>();
        Main.m_UI.GetOpenedUI<ModelUILogic>().ShowModel(model);
    }

    public void ShowModel(SimpleModel model)
    {
        Main.m_UI.OpenTemporaryUI<SimpleModelPanelUILogic>();
        Main.m_UI.GetOpenedUI<SimpleModelPanelUILogic>().ShowModel(model);
    }

    public void ShowTips(string content, string title = "提示：", float width = 200)
    {
        Main.m_UI.OpenTemporaryUI<TipsUILogic>(content, title, width);
    }

    public void HideTips()
    {
        Main.m_UI.GetOpenedUI<TipsUILogic>()?.UIEntity.GetComponent<TipsPanel>().Hide();
    }

    public void ShowIndicator<T>(string key, string message) where T : IndicatorBase
    {
        Main.m_UI.GetUI<T>().ShowIndicator(key, message);
        Main.m_UI.OpenResidentUI<T>();
    }

    public void HideIndicator<T>() where T : IndicatorBase
    {
        try
        {
            UIShowHideHelper.HideToRight(Main.m_UI.GetOpenedUI<T>().UIEntity);
            MainThread.Instance.DelayAndRun(300, Main.m_UI.CloseUI<T>);
        }
        catch { }
    }
}
