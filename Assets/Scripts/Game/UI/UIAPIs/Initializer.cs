using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAPIInitializer : SingletonBase<UIAPIInitializer>
{
    /// <summary>
    /// 初始化UI的API
    /// </summary>
    public void Initialize()
    {
        Main.m_UI.PreloadingResidentUI<StartUILogic>();

        Main.m_UI.PreloadingTemporaryUI<PauseUILogic>();

        Main.m_UI.PreloadingTemporaryUI<ModelUILogic>();
    }
}
