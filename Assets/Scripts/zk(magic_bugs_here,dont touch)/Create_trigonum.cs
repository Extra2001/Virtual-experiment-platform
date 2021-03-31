using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Create_trigonum : MonoBehaviour
{
    private GameObject Now_vernier;
    public GameObject My_trigonum;
    private GameObject Now_trigonum;
    IndicatorManager indicatorManager;
    private GameObject view;
    public bool on = false;
    Vector3 position = new Vector3(0.2f, 1.94f, -2.5f);
    Quaternion rotation = Quaternion.Euler(new Vector3(-90f, 0, 0));
    // Start is called before the first frame update
    void Start()
    {
        on = false;
        GetComponent<Button>().onClick.AddListener(Trigonum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Trigonum()
    {
        Now_trigonum = GameObject.Find("ruler");
        view = GameObject.Find("MainCamera");
        if (Now_trigonum.transform.position.x - 9f > 0)
        {
            view.GetComponent<Show_place>().name = "Ruler";
            Now_trigonum.transform.position = position;
            indicatorManager = GameObject.Find("Indicator").GetComponent<IndicatorManager>();
            GameObject.Find("MainCamera").GetComponent<Show_place>().Enable=true;
            indicatorManager.Indicator1.ShowIndicate("B", "贴近测量");
            if (Manager.state.Value < 3)
            {
                Manager.state.Value = 3;
            }
            on = true;
            if (GameObject.Find("book_0001b").GetComponent<Self_s>().t == 0)
            {
                GameObject.Find("book_0001b").GetComponent<Self_s>().t = 1;
            }
            if (GameObject.Find("book_0001b").GetComponent<Self_s>().t == 2)
            {
                GameObject.Find("book_0001b").GetComponent<Self_s>().t = 1;
                Now_vernier = GameObject.Find("Real_Vernier");
                Now_vernier.GetComponent<Move_vernier>().enabled = false;
                Now_vernier.transform.position = new Vector3(10.2f, 2f, -2.5f);
                indicatorManager.Indicator2.HideIndicate();
                indicatorManager.Indicator3.HideIndicate();
            }
        }
        else
        {
            Now_trigonum.transform.position = new Vector3(13.2f, 2f, -2.5f);
            indicatorManager = GameObject.Find("Indicator").GetComponent<IndicatorManager>();
            if (GameObject.Find("MainCamera").GetComponent<Show_place>().name == "Ruler")
            {
                GameObject.Find("MainCamera").GetComponent<Show_place>().Enable = false;
            }
            indicatorManager.Indicator1.ShowIndicate("F", "解锁视角");
            indicatorManager.Indicator2.ShowIndicate("X", "切换旋转");
            on = false;
        }

        GameObject.Find("Dropdown").GetComponent<Dropdown>().value = 0;
    }
}
