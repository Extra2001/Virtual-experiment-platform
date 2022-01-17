using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionCalcFormat
{
    public string Value = "0";
    public string Digit = "0";//科学计数法10的几次方
    public string A = "1";//指数或幂数的a
    public string[] Angle = new string[3] { "0", "0", "0" };//度、分、秒
    public bool[] AngleKind = new bool[3] { true, true, true };
}
public class DealCalc3 : HTBehaviour
{
    public Button CalcButton;
    public Text Ans;

    public List<GameObject> Cells;
    public List<FunctionCalcFormat> CellValue = new List<FunctionCalcFormat>();
    public List<Toggle> toggles;
    private int CurrentFunctionIndex = -1;

    public InputField UserValue;
    public InputField UserValue2;
    public InputField UserDigit;
    public Button UserSwitchButton;
    public Button AnsSwitchButton;
    public Text Reason;
    private string _uservalue;
    private string _userdigit;
    private int _userstate;
    private int _ansstate = -1;
    //启用自动化
    protected override bool IsAutomate => true;

    // Start is called before the first frame update
    void Start()
    {
        Reason.text = "";

        //calcvalue初始化
        for (int i = 0; i < Cells.Count; i++)
        {
            if (i < 2)
            {
                Cells[i].GetComponent<FunctionCalcCell1>().id = i;
                Cells[i].GetComponent<FunctionCalcCell1>().root = this.gameObject;
            }
            else if (i < 5)
            {
                Cells[i].GetComponent<FunctionCalcCell2>().id = i;
                Cells[i].GetComponent<FunctionCalcCell2>().root = this.gameObject;
            }
            else
            {
                Cells[i].GetComponent<FunctionCalcCell3>().id = i;
                Cells[i].GetComponent<FunctionCalcCell3>().root = this.gameObject;
            }
            CellValue.Add(new FunctionCalcFormat());
        }

        //toggle初始化
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;
            toggles[i].isOn = false;
            toggles[i].onValueChanged.AddListener(IfOn =>
            {
                if (IfOn)
                {
                    CurrentFunctionIndex = index;
                    for (int j = 0; j < toggles.Count; j++)
                    {
                        if (j != index)
                        {
                            toggles[j].isOn = false;
                        }
                    }
                }
                else
                {
                    bool flag = false;
                    for (int j = 0; j < toggles.Count; j++)
                    {
                        if (toggles[j].isOn)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        CurrentFunctionIndex = -1;
                    }
                }
            });
        }

        //用户答案输入初始化
        UserValue.onValueChanged.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                UserValue.text = "0";
            }
            else
            {
                _uservalue = value;
            }
        });
        UserDigit.onValueChanged.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                UserDigit.text = "0";
            }
            else if (value != "0" && int.Parse(value) - 0 == 0)
            {
                UserDigit.text = "0";
            }
            else
            {
                _userdigit = value;
            }
        });
        UserValue2.onValueChanged.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                UserValue2.text = "0";
            }
            else
            {
                //将正常数转换为科学计数法存储
                _uservalue = value;
                _userdigit = "0";
            }
        });
        UserSwitchButton.onClick.AddListener(() =>
        {
            UserValue.text = "0";
            UserDigit.text = "0";
            UserValue2.text = "0";

            _userstate = 1 - _userstate;
            if (_userstate == 0)
            {
                UserSwitchButton.FindChildren("Text").GetComponent<Text>().text = @"a <=> a*10^(b)";
                UserValue2.gameObject.SetActive(true);
            }
            else
            {
                UserSwitchButton.FindChildren("Text").GetComponent<Text>().text = @"a*10^(b) <=> a";
                UserValue2.gameObject.SetActive(false);
            }
        });
        AnsSwitchButton.onClick.AddListener(() =>
        {
            if (_ansstate != -1)
            {
                _ansstate = 1 - _ansstate;
                //将ans格式转换
            }
        });


    }


}
