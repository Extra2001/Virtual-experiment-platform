using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public float Sensitivety_mouse_wheel = 10f;
    private float Field_of_view;
    private GameObject player;

    private bool LookAble = true;

    public RotationAxes Axes = RotationAxes.MouseXAndY;
    public float Speed_v = 9.0f;
    public float Speed_h = 9.0f;
    public float Max_head = 45.0f;
    public float Min_head = -45.0f;
    private float Rotation_x = 0.0f;
    private float Rotation_y;
    // Start is called before the first frame update
    void Start()
    {
        Field_of_view = GetComponent<Camera>().fieldOfView;
        Rotation_y = GameObject.Find("Player").GetComponent<Transform>().localEulerAngles.y;
        player = GameObject.Find("Player");
        Main.m_Event.Subscribe<Sitdown>(WhenSitdown);
    }
    void LateUpdate()
    {

        if (LookAble)
        {
            Lookaround();
        }
    }
    void Lookaround()
    {
        //滚轮实现镜头缩进和拉远
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Field_of_view = Field_of_view - Input.GetAxis("Mouse ScrollWheel") * Sensitivety_mouse_wheel;
            if (Field_of_view < 10f)
            {
                Field_of_view = 10f;
            }
            else if (Field_of_view > 60f)
            {
                Field_of_view = 60f;
            }
            GetComponent<Camera>().fieldOfView = Field_of_view;

        }

        //视角随鼠标移动
        if (Axes == RotationAxes.MouseXAndY)
        {
            Rotation_x -= Input.GetAxis("Mouse Y") * Speed_v;//注意是-=
            Rotation_x = Mathf.Clamp(Rotation_x, Min_head, Max_head);//水平Verital
            Rotation_y = Rotation_y + Input.GetAxis("Mouse X") * Speed_h;
            transform.localEulerAngles = new Vector3(Rotation_x, 0, 0);
            player.GetComponent<Transform>().localEulerAngles = new Vector3(0, Rotation_y, 0);
        }
    }
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    private void WhenSitdown(object sender, EventHandlerBase handler)
    {
        Speed_h /= 20;
        Speed_v /= 10;
        Rotation_y = 180;
    }
}
