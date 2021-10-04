/************************************************************************************
    ���ߣ�������
    ��������Ϸ��ͣ��ʼ������
*************************************************************************************/
using HT.Framework;
using UnityEngine;

public class PauseManager : SingletonBehaviorManager<PauseManager>
{
    private void Start()
    {
        KeyboardManager.Instance.RegisterPermanant(KeyCode.Escape, () =>
        {
            if (Main.Current.Pause) Continue(true);
            else Pause(true);
        });
    }

    public void Pause(bool needOpenPauseMenu = false)
    {
        Main.Current.Pause = true;
        Time.timeScale = 0;
        Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<PauseEventHandler>().Fill(true, needOpenPauseMenu));
    }

    public void Continue(bool needOpenPauseMenu = false)
    {
        Main.Current.Pause = false;
        Time.timeScale = 1;
        Main.m_Event.Throw(this, Main.m_ReferencePool.Spawn<PauseEventHandler>().Fill(false, needOpenPauseMenu));
    }
}
