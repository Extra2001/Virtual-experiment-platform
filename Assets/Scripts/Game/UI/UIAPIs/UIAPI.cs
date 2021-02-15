using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAPI : SingletonBehaviorManager<UIAPI>
{
    public void ShowModel(ModelDialogModel model)
    {
        Main.m_UI.OpenTemporaryUI<ModelUILogic>();
        Main.m_UI.GetOpenedUI<ModelUILogic>().ShowModel(model);
    }
}
