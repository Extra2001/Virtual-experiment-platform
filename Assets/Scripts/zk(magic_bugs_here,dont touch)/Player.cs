using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Player_speed = 5f;
    private Rigidbody Body;
    public float Force = 5;

    void Start()
    {
        Body = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (Manager.Move_able == 1)
        {
            Move();
        }
    }
    void Move()
    {
        if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.UpArrow)) //前
        {
            this.transform.Translate(Vector3.forward * Player_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.DownArrow)) //后
        {
            this.transform.Translate(Vector3.forward * -Player_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)) //左
        {
            this.transform.Translate(Vector3.right * -Player_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow)) //右
        {
            this.transform.Translate(Vector3.right * Player_speed * Time.deltaTime);
        }

        //跳跃
        if (float.Parse(string.Format("{0:F1}", transform.position.y)) > 1.6f)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Body.velocity += Vector3.up * Force;
        }
    }



}
