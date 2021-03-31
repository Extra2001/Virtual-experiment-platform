using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Open_door : MonoBehaviour
{
    private bool Is_open = false;
    private GameObject Door;
    private GameObject Player;

    IndicatorManager indicatorManager;
    // Start is called before the first frame update
    void Start()
    {
        Door = GameObject.Find("Door_hint");
        Player = GameObject.Find("Player");

        indicatorManager = GameObject.Find("Indicator").GetComponent<IndicatorManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        If_end();
    }
    void OnTriggerEnter(Collider other)
    {
        if (Manager.state.Value == 5)
        {
            indicatorManager.Indicator1.ShowIndicate("E", "开门结束实验");
        }
        
    }
    void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E)&&Manager.state.Value==5)
        {
            Open_classroom_door();
            Is_open = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        indicatorManager.HideAllIndicator();
        Close_classroom_door();
        Is_open = false;
    }

    void Open_classroom_door()
    {
        if (Player.transform.position.z < 3.7f)
        {
            Door.transform.localEulerAngles = new Vector3(0, -75, 0);
        }else
        {
            Door.transform.localEulerAngles = new Vector3(0, 75, 0);
        }
        
    }
    void Close_classroom_door()
    {
        Door.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
    void If_end()
    {
        if (Is_open && Player.transform.position.z > 4.5f)
        {
            SceneManager.LoadScene("End");
        }
    }

}
