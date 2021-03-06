using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PauseManager : SingletonBehaviorManager<PauseManager>
{
    private void Start()
    {
        KeyboardManager.Instance.Register(KeyCode.Escape, () =>
        {
            if (Main.Current.Pause)
                Continue();
            else
                Pause();
        });
    }

    /// <summary>
    /// ��ͣ��Ϸ
    /// </summary>
    public void Pause()
    {
        Main.Current.Pause = true;
        Time.timeScale = 0;
        Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<PauseEventHandler>().Fill(true));
    }
    public void Pause(bool needOpenPauseMenu)
    {
        Main.Current.Pause = true;
        Time.timeScale = 0;
        Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<PauseEventHandler>().Fill(true, needOpenPauseMenu));
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void Continue()
    {
        Main.Current.Pause = false;
        Time.timeScale = 1;
        Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<PauseEventHandler>().Fill(false));
    }

    public void Continue(bool needOpenPauseMenu)
    {
        Main.Current.Pause = false;
        Time.timeScale = 1;
        Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<PauseEventHandler>().Fill(false, needOpenPauseMenu));
    }
}
