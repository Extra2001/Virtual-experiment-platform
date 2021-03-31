using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_tri : MonoBehaviour
{
    private float distance = 0.5f;
    private Vector3 dist;
    private float posX;
    private float posY;
    private Vector3 AOTIScreen; //用来获取物体在屏幕的坐标
    private Vector3 MouseStartScreen; //用来获取鼠标按下时在屏幕的位置
    private Vector3 MouseNewScreen; //用来获取鼠标按下时每帧的位置
    private Vector3 valueScreen; //用来获取偏移量
    private float OffsetX = 0;
    private float OffsetY = 0;
    public float speed = 6f;//旋转速度
    private bool IsGet = false;
    private bool Ismove = false;
    private float Time_T = 3.0f;
    private bool enable_move = true;
    private bool On_exam = false;

    private Tuple<string, string> LookLockedIndicate;
    private IndicatorManager indicatorManager;
    // Start is called before the first frame update
    private void Start()
    {
        indicatorManager = GameObject.Find("Indicator").GetComponent<IndicatorManager>();
    }

    private void OnMouseEnter()
    {
        IsGet = true;
    }
    private void OnMouseExit()
    {
        IsGet = false;
        if (!Input.GetMouseButton(1))
        {
            Ismove = false;
        }
    }
    private void Move()
    {
        //1.首先将物体从世界坐标转为屏幕坐标
        AOTIScreen = Camera.main.WorldToScreenPoint(transform.position);
        //2.获取鼠标在屏幕坐标的偏移量
        if (Input.GetMouseButtonDown(0))
        {
            MouseStartScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, AOTIScreen.z);
        }
        MouseNewScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, AOTIScreen.z);
        if (MouseNewScreen != MouseStartScreen)
        {
            valueScreen = MouseNewScreen - MouseStartScreen;
            //3.将偏移量给物体
            transform.position = Camera.main.ScreenToWorldPoint(AOTIScreen + valueScreen);
        }
        MouseStartScreen = MouseNewScreen;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "book")
        {
            enable_move=false;
            On_exam = true;
            indicatorManager.Indicator1.ShowIndicate("A", "进行测量");
            indicatorManager.Indicator2.ShowIndicate("S", "放弃测量");
            LookLockedIndicate =Tuple.Create(indicatorManager.Indicator1.keyText, indicatorManager.Indicator1.indicateText);
        }
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.S)&&On_exam)
        {
            transform.position = new Vector3(0, 1.94f, -3f);
            enable_move = true;
            On_exam = false;
            indicatorManager.Indicator1.ShowIndicate(LookLockedIndicate.Item1, LookLockedIndicate.Item2);
        }
        if (Input.GetKey(KeyCode.A) && On_exam)
        {
            GameObject Cam = GameObject.Find("MainCamera");
            GameObject ruler = GameObject.Find("ruler(Clone)");
            print(this.transform.eulerAngles);
            print(Cam.transform.eulerAngles);
            Cam.transform.rotation = Quaternion.Euler(this.transform.eulerAngles);
        }
        if (enable_move)
        {
            if (Input.GetMouseButton(1) && (IsGet || Ismove))
            {
                Ismove = true;
                OffsetX = Input.GetAxis("Mouse X");//获取鼠标x轴的偏移量
                OffsetY = Input.GetAxis("Mouse Y");//获取鼠标y轴的偏移量
                transform.Rotate(new Vector3(OffsetY, -OffsetX, 0) * speed, Space.World);
            }
        }

        if (!Input.GetMouseButton(1))
        {
            if (!Input.GetMouseButton(0))
            {
                if (transform.eulerAngles.x < 30 && transform.eulerAngles.x > 1)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x - 1, transform.eulerAngles.y, transform.eulerAngles.z);
                if (transform.eulerAngles.x <= 1 && transform.eulerAngles.x > 0)
                    transform.rotation = Quaternion.Euler(360.0f, transform.eulerAngles.y, transform.eulerAngles.z);
                if (transform.eulerAngles.x > 330 && transform.eulerAngles.x < 359)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x + 1, transform.eulerAngles.y, transform.eulerAngles.z);
                if (transform.eulerAngles.x >= 359)
                    transform.rotation = Quaternion.Euler(360.0f, transform.eulerAngles.y, transform.eulerAngles.z);

                if (transform.eulerAngles.x < 89 && transform.eulerAngles.x > 60)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x + 1, transform.eulerAngles.y, transform.eulerAngles.z);
                if (transform.eulerAngles.x <= 90 && transform.eulerAngles.x >= 89)
                    transform.rotation = Quaternion.Euler(90.0f, transform.eulerAngles.y, transform.eulerAngles.z);

                if (transform.eulerAngles.x < 300 && transform.eulerAngles.x > 271)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x - 1, transform.eulerAngles.y, transform.eulerAngles.z);
                if (transform.eulerAngles.x <= 271 && transform.eulerAngles.x >= 270)
                    transform.rotation = Quaternion.Euler(270.0f, transform.eulerAngles.y, transform.eulerAngles.z);



                if (transform.eulerAngles.y < 30 && transform.eulerAngles.y > 1)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - 1, transform.eulerAngles.z);
                if (transform.eulerAngles.y <= 1)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0.0f, transform.eulerAngles.z);
                if (transform.eulerAngles.y > 330 && transform.eulerAngles.y < 359)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 1, transform.eulerAngles.z);
                if (transform.eulerAngles.y >= 359)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 360.0f, transform.eulerAngles.z);

                if (transform.eulerAngles.y < 89 && transform.eulerAngles.y > 60)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 1, transform.eulerAngles.z);
                if (transform.eulerAngles.y <= 120 && transform.eulerAngles.y >= 91)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - 1, transform.eulerAngles.z);
                if (transform.eulerAngles.y < 91 && transform.eulerAngles.y > 89)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 90.0f, transform.eulerAngles.z);

                if (transform.eulerAngles.y < 179 && transform.eulerAngles.y > 150)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 1, transform.eulerAngles.z);
                if (transform.eulerAngles.y <= 210 && transform.eulerAngles.y >= 181)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - 1, transform.eulerAngles.z);
                if (transform.eulerAngles.y < 181 && transform.eulerAngles.y > 179)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180.0f, transform.eulerAngles.z);

                if (transform.eulerAngles.y < 269 && transform.eulerAngles.y > 240)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 1, transform.eulerAngles.z);
                if (transform.eulerAngles.y <= 300 && transform.eulerAngles.y >= 271)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - 1, transform.eulerAngles.z);
                if (transform.eulerAngles.y < 271 && transform.eulerAngles.y > 269)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 270.0f, transform.eulerAngles.z);





                if (transform.eulerAngles.z < 30 && transform.eulerAngles.z > 1)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 1);
                if (transform.eulerAngles.z <= 1)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
                if (transform.eulerAngles.z > 330 && transform.eulerAngles.z < 359)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 1);
                if (transform.eulerAngles.z >= 359)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 360.0f);

                if (transform.eulerAngles.z < 89 && transform.eulerAngles.z > 60)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 1);
                if (transform.eulerAngles.z <= 120 && transform.eulerAngles.z >= 91)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 1);
                if (transform.eulerAngles.z < 91 && transform.eulerAngles.z > 89)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 90.0f);

                if (transform.eulerAngles.z < 179 && transform.eulerAngles.z > 150)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 1);
                if (transform.eulerAngles.z <= 210 && transform.eulerAngles.z >= 181)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 1);
                if (transform.eulerAngles.z < 181 && transform.eulerAngles.z > 179)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 180.0f);

                if (transform.eulerAngles.z < 269 && transform.eulerAngles.z > 240)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 1);
                if (transform.eulerAngles.z <= 300 && transform.eulerAngles.z >= 271)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 1);
                if (transform.eulerAngles.z < 271 && transform.eulerAngles.z > 269)
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 270.0f);

            }
        }
        //if (!Input.GetMouseButton(1))
        //{
        //    if (!Input.GetMouseButton(0))
        //    {
        //        print(transform.rotation.x);
        //        if (transform.rotation.x > 150 && transform.rotation.x < 210)
        //        {
        //            print("okk");
        //            transform.rotation = Quaternion.Euler(new Vector3(180, transform.rotation.y, transform.rotation.z));
        //        }
        //    }
        //}
    }

    //private void OnMouseDown()
    //{
    //    dist = Camera.main.WorldToScreenPoint(transform.position);
    //    posX = Input.mousePosition.x - dist.x;
    //    posY = Input.mousePosition.y - dist.y;
    //}
    //private void OnMouseDrag()
    //{
    //    Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
    //    Vector3 worldPos = Camera.main.WorldToScreenPoint(curPos);
    //    transform.position = worldPos;
    //}

    void OnMouseDrag()
    {
        if (enable_move)
        {
            //获取到鼠标的位置(鼠标水平的输入和竖直的输入以及距离)
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            //物体的位置，屏幕坐标转换为世界坐标
            Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            //把鼠标位置传给物体
            transform.position = objectPosition;
        }
    }
}
