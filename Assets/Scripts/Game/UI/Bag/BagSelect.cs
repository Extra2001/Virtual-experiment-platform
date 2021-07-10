/************************************************************************************
    作者：张峻凡
    描述：切换仪器和被测物体的处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class BagSelect : HTBehaviour
{
    public SegmentedControl BagControl;

    public GameObject Instruments;
    public GameObject Objects;
    public Text title;

    void Start()
    {
        BagControl.onValueChanged.AddListener(OnChoose);
    }
    /// <summary>
    /// 切换仪器和被测物体
    /// </summary>
    /// <param name="value"></param>
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
