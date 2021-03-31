using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Create_vernier : MonoBehaviour
{
    public GameObject My_vernier;
    private GameObject Now_vernier;
    private GameObject Now_vernier1;
    private GameObject Now_vernier2;
    public bool on = false; 
    private Vector3 Vv1;
    private GameObject view;
    private GameObject Now_trigonum;
    private Vector3 Vv2;
    Vector3 position = new Vector3(0.3f, 2f, -1.8f);
    Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));

    IndicatorManager indicatorManager;

    // Start is called before the first frame update
    void Start()
    {
        on = false;
        GetComponent<Button>().onClick.AddListener(Vernier);
        Now_vernier = GameObject.Find("Real_Vernier");
        indicatorManager = GameObject.Find("Indicator").GetComponent<IndicatorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Vernier()
    {

        if (Now_vernier.transform.position.x-9f>0)
        {
            Now_vernier.transform.position = position;
            GameObject.Find("MeasureHead").transform.position = new Vector3(0.3f, 2.0f, -1.6f);
            GameObject.Find("MeasureBody").transform.position = new Vector3(0.3f, 2.0f, -1.8f);
            Now_vernier.GetComponent<Move_vernier>().enabled = true;
            Now_vernier.GetComponent<Move_vernier>().move1 = true;
            Now_vernier.GetComponent<Move_vernier>().move2 = true;
            view = GameObject.Find("MainCamera");
            view.GetComponent<Show_place>().name = "Vernier";
            indicatorManager = GameObject.Find("Indicator").GetComponent<IndicatorManager>();
            indicatorManager.Indicator1.ShowIndicate("B", "贴近测量");
            GameObject.Find("MainCamera").GetComponent<Show_place>().Enable = true;
            indicatorManager.Indicator2.ShowIndicate("O", "收紧卡尺");
            indicatorManager.Indicator3.ShowIndicate("P", "拉伸卡尺");
            if (Manager.state.Value < 3)
            {
                Manager.state.Value = 3;
            }           
            on = true;
            if (GameObject.Find("book_0001b").GetComponent<Self_s>().t == 0)
            {
                GameObject.Find("book_0001b").GetComponent<Self_s>().t = 2;
            }
            if (GameObject.Find("book_0001b").GetComponent<Self_s>().t == 1)
            {
                GameObject.Find("book_0001b").GetComponent<Self_s>().t = 2;
                Now_vernier = GameObject.Find("Real_Vernier");
                Now_trigonum = GameObject.Find("ruler");
                Now_trigonum.transform.position = new Vector3(13.2f, 2f, -2.5f);
            }
        }
        else
        {
            Now_vernier.GetComponent<Move_vernier>().enabled = false;
            Now_vernier.transform.position= new Vector3(10.2f, 2f, -2.5f);
            indicatorManager = GameObject.Find("Indicator").GetComponent<IndicatorManager>();
            indicatorManager.Indicator1.ShowIndicate("F", "解锁视角");
            if (GameObject.Find("MainCamera").GetComponent<Show_place>().name == "Vernier")
            {
                GameObject.Find("MainCamera").GetComponent<Show_place>().Enable = false;
            }
            indicatorManager.Indicator2.ShowIndicate("X", "切换旋转");
            indicatorManager.Indicator3.HideIndicate();
            on = false;
        }

        GameObject.Find("Dropdown").GetComponent<Dropdown>().value = 0;
    }
}
