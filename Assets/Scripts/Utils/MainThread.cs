/************************************************************************************
    ���ߣ�������
    ��������Unity���߳������к���
*************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class MainThread : SingletonBehaviorManager<MainThread>
{
    /// <summary>
    /// Ҫ���е�����
    /// </summary>
    List<Model> models = new List<Model>();

    /// <summary>
    /// ÿ֡ʱ������Ƿ�������Ҫ����
    /// </summary>
    void Update()
    {
        long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        for (int i = models.Count - 1; i >= 0; i--)
        {
            if (now - models[i].startTime > models[i].delay)
            {
                if (models[i].action != null)
                    models[i].action.Invoke();
                models.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// �����߳�������
    /// </summary>
    public void Run(Action action)
    {
        DelayAndRun(0, action);
    }
    /// <summary>
    /// �����߳����ӳٲ�����
    /// </summary>
    public void DelayAndRun(int delay, Action action)
    {
        long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        models.Add(new Model()
        {
            startTime = now,
            delay = delay,
            action = action
        });
    }
    public class Model
    {
        public int delay;
        public long startTime;
        public Action action;
    }
}
