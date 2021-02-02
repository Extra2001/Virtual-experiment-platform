using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);

        LaunchServices();

        LaunchManagers();
    }

    private void LaunchServices() {
        // 启动服务程序
    }

    private void LaunchManagers() {
        GameManager.Enable();
    }
}
