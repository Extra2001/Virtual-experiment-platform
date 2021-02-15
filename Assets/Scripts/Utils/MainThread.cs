using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class MainThread : SingletonBehaviorManager<MainThread>
{
    List<Action> tasks = new List<Action>();

    // Update is called once per frame
    void Update()
    {
        for(int i=tasks.Count - 1; i >= 0; i--)
        {
            tasks[i].Invoke();
            tasks.RemoveAt(i);
        }
    }

    public void Run(Action action)
    {
        tasks.Add(action);
    }
}
