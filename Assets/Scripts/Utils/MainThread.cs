/************************************************************************************
    作者：荆煦添
    描述：在Unity主线程上运行函数
*************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;

public class MainThread : SingletonBehaviorManager<MainThread>
{
    /// <summary>
    /// 要运行的任务
    /// </summary>
    List<Action> tasks = new List<Action>();

    /// <summary>
    /// 每帧时都检测是否有任务要运行
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
    /// 在主线程上运行
    /// </summary>
    public void Run(Action action)
    {
        tasks.Add(action);
    }
    /// <summary>
    /// 在主线程上延迟并运行
    /// </summary>
    public Task DelayAndRun(int delay, Action action)
    {
        return Task.Delay(delay).ContinueWith(_ =>
        {
            Run(action);
        });
    }
    /// <summary>
    /// 在主线程上延迟并运行，可配置取消
    /// </summary>
    public Task DelayAndRun(int delay, CancellationToken cancellationToken, Action action)
    {
        return Task.Delay(delay, cancellationToken).ContinueWith(_ =>
        {
            Run(action);
        });
    }
}
