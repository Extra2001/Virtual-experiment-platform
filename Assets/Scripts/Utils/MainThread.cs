/************************************************************************************
    ���ߣ�������
    ��������Unity���߳������к���
*************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;

public class MainThread : SingletonBehaviorManager<MainThread>
{
    /// <summary>
    /// Ҫ���е�����
    /// </summary>
    List<Action> tasks = new List<Action>();

    /// <summary>
    /// ÿ֡ʱ������Ƿ�������Ҫ����
    /// </summary>
    void Update()
    {
        for(int i=tasks.Count - 1; i >= 0; i--)
        {
            tasks[i].Invoke();
            tasks.RemoveAt(i);
        }
    }
    /// <summary>
    /// �����߳�������
    /// </summary>
    public void Run(Action action)
    {
        tasks.Add(action);
    }
    /// <summary>
    /// �����߳����ӳٲ�����
    /// </summary>
    public Task DelayAndRun(int delay, Action action)
    {
        return Task.Delay(delay).ContinueWith(_ =>
        {
            Run(action);
        });
    }
    /// <summary>
    /// �����߳����ӳٲ����У�������ȡ��
    /// </summary>
    public Task DelayAndRun(int delay, CancellationToken cancellationToken, Action action)
    {
        return Task.Delay(delay, cancellationToken).ContinueWith(_ =>
        {
            Run(action);
        });
    }
}
