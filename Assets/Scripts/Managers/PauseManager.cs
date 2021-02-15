using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PauseManager : SingletonBehaviorManager<PauseManager>
{
    private bool canSetEvent = true;

    private void Update()
    {
        if (canSetEvent && Input.GetKey(KeyCode.Escape))
        {
            if (Main.Current.Pause)
            {
                Continue();
            }
            else
            {
                Pause();
            }
            canSetEvent = false;
            Task.Delay(300).ContinueWith((_) =>
            {
                canSetEvent = true;
            });
        }
    }

    /// <summary>
    /// ‘›Õ£”Œœ∑
    /// </summary>
    public void Pause()
    {
        Main.Current.Pause = true;
        Time.timeScale = 0;
        Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<PauseEventHandler>().Fill(true));
    }

    /// <summary>
    /// ºÃ–¯”Œœ∑
    /// </summary>
    public void Continue()
    {
        Main.Current.Pause = false;
        Time.timeScale = 1;
        Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<PauseEventHandler>().Fill(false));
    }
}
