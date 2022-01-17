using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DealCalc3 : HTBehaviour
{
    public Button CalcButton;
    public Text Ans;

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
        
        /*CalcButton.onClick.AddListener(() =>
        {
            bool finish = true;
            //检查算式
            foreach (var item in Cells)
            {
                var temp = item.GetComponent<MultiplayAndDivideCell>();
                if (temp.state == 1)
                {
                    if (temp.Value.text == "0" || temp.Digit.text == "0")
                    {
                        finish = false;
                    }
                    if (double.Parse(CellValue[temp.id].Value) - 1 < 0 || double.Parse(CellValue[temp.id].Value) - 10 >= 0)
                    {
                        finish = false;
                    }
                }
                else
                {
                    if (temp.Value2.text == "0")
                    {
                        finish = false;
                    }
                }
            }
            //检查答案输入
            if (_userstate == 0)
            {
                if (UserValue2.text == "0")
                {
                    finish = false;
                }
            }
            else
            {
                if (UserValue.text == "0" || UserDigit.text == "0")
                {
                    finish = false;
                }
            }

            if (!finish)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Message = "请检查输入的算式是否完整正确无冗余",
                    ShowCancel = false
                });
            }
            else
            {
                List<(string rawnumstr, int isadd)> input = new List<(string rawnumstr, int isadd)>();
                (string rawnumstr, int isadd) temp;
                for (int i = 0; i < CellValue.Count; i++)
                {
                    temp.isadd = CellValue[i].Sign;
                    temp.rawnumstr = StaticMethods.SciToExp(CellValue[i].Value + "*10^(" + CellValue[i].Digit + ")");
                    input.Add(temp);
                }
                string userresult = StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")");
                (bool correct, string message, CheckFloat2 correctvalue) = CheckFloat2.CheckGroupMul(input, userresult);

                if (!correct)
                {
                    Reason.text = message;
                }
                else
                {
                    Reason.text = "计算正确";
                }
                Ans.text = correctvalue.ToString();
                _ansstate = 1;
            }
        });*/

        UserValue.onValueChanged.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                UserValue.text = "0";
            }
            else if (value != "0" && double.Parse(value) - 0 == 0)
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
            else if (value != "0" && double.Parse(value) - 0 == 0)
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
