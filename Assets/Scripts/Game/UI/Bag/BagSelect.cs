using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class BagSelect : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public SegmentedControl BagControl;

    public GameObject Instruments;
    public GameObject Objects;
    public Text title;

    // Start is called before the first frame update
    void Start()
    {
        //Instruments = transform.Find("Select_instrument").gameObject;
        //Objects = transform.Find("Select_object").gameObject;
        //title = transform.Find("BagControl").gameObject.transform.Find("Title").GetComponent<Text>();

        //SegmentedControl BagControl = transform.Find("BagControl").GetComponent<SegmentedControl>();
        BagControl.onValueChanged.AddListener(OnChoose);
    }


    private void OnChoose(int value)
    {
        if (value < 1)
        {
            //title.text = "测量工具";
            Instruments.SetActive(true);
            Objects.SetActive(false);
        }
        else if (value == 1)
        {
            //title.text = "被测物体";
            Instruments.SetActive(false);
            Objects.SetActive(true);
        }
    }
}
