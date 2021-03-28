using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flexible_menu : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    private int screenx;
    private int screeny;
    private float distance;
    private bool Ifshow = false;
    private float Mousex;

    // Start is called before the first frame update
    void Start()
    {
        screenx = Screen.width;
        screeny = Screen.height;
        distance = GetComponent<RectTransform>().rect.width;
        GetComponent<RectTransform>().offsetMax = new Vector2(-distance, 0);//此处有bug，负号不起作用
        GetComponent<RectTransform>().offsetMin = new Vector2(screenx+distance, 0);
        Debug.Log(screenx);
        Debug.Log(screenx);
        Debug.Log(distance);
    }

    // Update is called once per frame
    void Update()
    {
        Mousex = Input.mousePosition.x;
        Debug.Log("x:"+Mousex);
        if (Ifshow)
        {
            if (Mousex < screenx-distance)
            {
                hide();
            }
        }
        else
        {
            if (Mousex > screenx-50)
            {
                show();
            }
        }
    }

    void show()
    {
        Debug.Log("show");
        if (Ifshow == false)
        {
            GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            GetComponent<RectTransform>().offsetMin = new Vector2(screenx, 0);
            Ifshow = true;
        }
        
    }
    void hide()
    {
        Debug.Log("hide");
        if (Ifshow == true)
        {
            GetComponent<RectTransform>().offsetMax = new Vector2(-distance, 0);
            GetComponent<RectTransform>().offsetMin = new Vector2(screenx+distance, 0);
            Ifshow = false;
        }
        
    }
}
