using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MixCalcControl : HTBehaviour
{
    public Button StartButton;

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

    public int NumRealLength = 0;//记录正确的有效数字有多少位
    public string LastValue;//混合计算的上一个值
    public List<CheckFloat2> HistoryResult = new List<CheckFloat2>();

    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        Reason.text = "";
        StartButton.onClick.AddListener(() =>
        {
            NumRealLength = 0;
            LastValue = string.Empty;
            HistoryResult = new List<CheckFloat2>();
        });
        CalcButton.onClick.AddListener(() =>
        {
            if (NumRealLength == 0)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Title = "警告",
                    Message = "混合运算至少应当进行一次运算",
                    ShowCancel = false
                });
            }
            else
            {
                bool finish = true;
                //检查答案输入
                if (_userstate == 0)
                {
                    if (string.IsNullOrEmpty(UserValue2.text))
                    {
                        finish = false;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(UserValue.text) || string.IsNullOrEmpty(UserDigit.text))
                    {
                        finish = false;
                    }
                }
                if (!finish)
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Title = "警告",
                        Message = "请输入你认为的答案",
                        ShowCancel = false
                    });
                    return;
                }
                string ShowValue = new CheckFloat2(CheckFloat2.KeepEffective(decimal.Parse(LastValue), NumRealLength)).ToString();
                if (ShowValue.Contains("E") || ShowValue.Contains("e"))
                {
                    _ansstate = 1;
                    Ans.text = StaticMethods.ExpToSci(ShowValue);
                }
                else
                {
                    _ansstate = 0;
                    Ans.text = ShowValue;
                }
                CheckFloat2 _ans = new CheckFloat2(ShowValue);
                string userresult = StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")");
                CheckFloat2 _user = new CheckFloat2(userresult);
                bool correct = (_ans.EffectiveDigit == _user.EffectiveDigit) && (_ans.Value - _user.Value == 0);

                if (correct)
                {
                    Reason.text = "计算正确";
                }
                else
                {
                    Reason.text = "计算错误，混合运算中间结果需多保留1-2位，最终结果才进行截断";
                }
            }
        });

        //用户答案输入初始化
        UserValue.onValueChanged.AddListener(value =>
        {
            if ((!string.IsNullOrEmpty(value)) && double.TryParse(value, out double t))
            {
                if ((Math.Abs(double.Parse(value)) - 1 >= 0) && (Math.Abs(double.Parse(value)) - 10 < 0))
                {
                    _uservalue = value;
                }
                else
                {
                    UserValue.text = string.Empty;
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Title = "警告",
                        Message = "请输入合法的数字",
                        ShowCancel = false
                    });
                }
            }
        });
        UserDigit.onValueChanged.AddListener(value =>
        {
            if ((!string.IsNullOrEmpty(value)) && double.TryParse(value, out double t))
            {
                if (int.Parse(value) != 0)
                {
                    _userdigit = value;
                    return;
                }
                else
                {
                    UserDigit.text = string.Empty;
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Title = "警告",
                        Message = "请输入合法的数字",
                        ShowCancel = false
                    });
                }
            }
        });
        UserValue2.onValueChanged.AddListener(value =>
        {
            if ((!string.IsNullOrEmpty(value)) && double.TryParse(value, out double t))
            {
                _uservalue = value;
                _userdigit = "0";
            }
        });
        UserSwitchButton.onClick.AddListener(() =>
        {
            UserValue.text = string.Empty;
            UserDigit.text = string.Empty;
            UserValue2.text = string.Empty;

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
                if (_ansstate == 0)
                {
                    if (string.IsNullOrEmpty(StaticMethods.NormToExp(Ans.text)))
                    {
                        UIAPI.Instance.ShowModel(new SimpleModel()
                        {
                            Title = "警告",
                            Message = "答案不能用科学计数法表示",
                            ShowCancel = false
                        });
                        _ansstate = 0;
                    }
                    else
                    {
                        Ans.text = StaticMethods.ExpToSci(StaticMethods.NormToExp(Ans.text));
                        _ansstate = 1;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(StaticMethods.ExpToNorm(StaticMethods.SciToExp(Ans.text))))
                    {
                        UIAPI.Instance.ShowModel(new SimpleModel()
                        {
                            Title = "警告",
                            Message = "答案只能用科学计数法表示",
                            ShowCancel = false
                        });
                        _ansstate = 1;
                    }
                    else
                    {
                        Ans.text = StaticMethods.ExpToNorm(StaticMethods.SciToExp(Ans.text));
                        _ansstate = 0;
                    }
                }


            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
