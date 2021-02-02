﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    public static T Instance
    {
        get => GetInstance();
    }

    private static bool applicationIsQuitting = false;

    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }

    public static void Enable()
    {
        GetInstance();
    }

    private static T GetInstance()
    {
        if (applicationIsQuitting)
        {
            return null;
        }
    
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));

                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = "(Singleton) " + typeof(T).ToString();
                    DontDestroyOnLoad(singleton);
                }
            }

            return _instance;
        }
    }
}