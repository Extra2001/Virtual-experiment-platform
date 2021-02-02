using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        LoadStartScreen();
    }

    public void LoadStartScreen()
    {
        Debug.Log("启动啦！");
    }

    public void LoadMainScene()
    {

    }
}
