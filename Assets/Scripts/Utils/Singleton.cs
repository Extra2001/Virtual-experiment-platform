using HT.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 已弃用，请使用HT.Framework.SingletonBase<>

//public class Singleton<T> : SingletonBase<T> where T : class, new()
//{
//    private static T _instance;

//    private static object _lock = new object();

//    public static T Instance
//    {
//        get => GetInstance();
//    }

//    private static bool applicationIsQuitting = false;

//    public virtual void OnDestroy()
//    {
//        applicationIsQuitting = true;
//    }

//    public static void Enable()
//    {
//        GetInstance();
//    }

//    private static T GetInstance()
//    {
//        if (applicationIsQuitting)
//        {
//            return null;
//        }
    
//        lock (_lock)
//        {
//            if (_instance == null)
//            {
//                _instance = (T)FindObjectOfType(typeof(T));

//                if (FindObjectsOfType(typeof(T)).Length > 1)
//                {
//                    return _instance;
//                }

//                if (_instance == null)
//                {
//                    GameObject singleton = new GameObject();
//                    _instance = singleton.AddComponent<T>();
//                    singleton.name = "(Singleton) " + typeof(T).ToString();
//                    DontDestroyOnLoad(singleton);
//                }
//            }

//            return _instance;
//        }
//    }
//}