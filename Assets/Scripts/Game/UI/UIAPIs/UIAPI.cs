using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UIAPI : SingletonBehaviorManager<UIAPI>
{
    public void ShowModel(ModelDialogModel model)
    {
        Main.m_UI.OpenTemporaryUI<ModelUILogic>();
        Main.m_UI.GetOpenedUI<ModelUILogic>().ShowModel(model);
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
            Task.Delay(300).ContinueWith(_ =>
            {
                MainThread.Instance.Run(() =>
                {
                    Main.m_UI.CloseUI<T>();
                });
            });
        }
        catch { }
    }
}
