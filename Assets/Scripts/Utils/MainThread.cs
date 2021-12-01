/************************************************************************************
    作者：荆煦添
    描述：在Unity主线程上运行函数
*************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class MainThread : SingletonBehaviorManager<MainThread>
{
    /// <summary>
    /// 要运行的任务
    /// </summary>
    List<Model> models = new List<Model>();

    /// <summary>
    /// 每帧时都检测是否有任务要运行
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
    /// 在主线程上运行
    /// </summary>
    public void Run(Action action)
    {
        DelayAndRun(0, action);
    }
    /// <summary>
    /// 在主线程上延迟并运行
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
