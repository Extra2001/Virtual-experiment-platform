/************************************************************************************
    来源：CSDN
    描述：配置单例模式运行的MonoBehavior
*************************************************************************************/
using UnityEngine;

public class SingletonBehaviorManager<T> : MonoBehaviour where T: Component
{
    private static T _instance;

    private static object _lock = new object();
    
    public static T Instance
    {
        get => GetInstance();
    }

    private static bool applicationIsQuitting = false;

    public virtual void OnDestroy()
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
                _instance = GameObject.Find("GameManager").GetComponent<T>();

                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    _instance = GameObject.Find("GameManager").AddComponent<T>();
                }
            }
            return _instance;
        }
    }
}