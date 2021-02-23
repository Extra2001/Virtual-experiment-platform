using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public float Player_speed = 5f;
    private bool Moveable = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Moveable)
        {
            Moving();
        }
    }

    private void Moving()
    {
        if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.UpArrow)) //前
        {
            transform.Translate(Vector3.forward * Player_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.DownArrow)) //后
        {
            transform.Translate(Vector3.forward * -Player_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)) //左
        {
            transform.Translate(Vector3.right * -Player_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow)) //右
        {
            transform.Translate(Vector3.right * Player_speed * Time.deltaTime);
        }
    }

}
