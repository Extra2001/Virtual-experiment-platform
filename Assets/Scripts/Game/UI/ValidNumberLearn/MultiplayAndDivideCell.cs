using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayAndDivideCell : HTBehaviour
{
    public int id;//对应id
    public GameObject Root;//父节点

    public Button Sign;
    public Button SwitchButton;
    public InputField Value;
    public InputField Value2;
    public InputField Digit;
    public Sprite MultiplayImage;
    public Sprite DivideImage;

    public int state = 0;//0代表正常数，1代表科学计数法
    //启用自动化
    protected override bool IsAutomate => true;

    // Start is called before the first frame update
    void Start()
    {
        Value.onEndEdit.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                Value.text = "0";
            }
            else if (value != "0" && double.Parse(value) - 0 == 0)
            {
                Value.text = "0";
            }
            else
            {
                Root.GetComponent<DealCalc2>().CellValue[id].Value = value;
            }
        });
        Digit.onEndEdit.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                Digit.text = "0";
            }
            else if (value != "0" && int.Parse(value) - 0 == 0)
            {
                Digit.text = "0";
            }
            else
            {
                Root.GetComponent<DealCalc2>().CellValue[id].Digit = value;
            }
        });
        Value2.onEndEdit.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                Value2.text = "0";
            }
            else if (value != "0" && double.Parse(value) - 0 == 0)
            {
                Value2.text = "0";
            }
            else
            {
                //将正常数转换为科学计数法存储
                Root.GetComponent<DealCalc2>().CellValue[id].Value = value;
                Root.GetComponent<DealCalc2>().CellValue[id].Digit = "0";
            }
        });

        Sign.onClick.AddListener(() =>
        {
            int temp = 1 - Root.GetComponent<DealCalc2>().CellValue[id].Sign;
            Root.GetComponent<DealCalc2>().CellValue[id].Sign = temp;
            if (temp == 0)
            {
                Sign.image.sprite = MultiplayImage;
            }
            else
            {
                Sign.image.sprite = DivideImage;
            }
        });
        SwitchButton.onClick.AddListener(() =>
        {
            Value.text = "0";
            Digit.text = "0";
            Value2.text = "0";

            state = 1 - state;
            if (state == 0)
            {
                SwitchButton.FindChildren("Text").GetComponent<Text>().text = @"a <=> a*10^(b)";
                Value2.gameObject.SetActive(true);
            }
            else
            {
                SwitchButton.FindChildren("Text").GetComponent<Text>().text = @"a*10^(b) <=> a";
                Value2.gameObject.SetActive(false);
            }
        });
    }


}