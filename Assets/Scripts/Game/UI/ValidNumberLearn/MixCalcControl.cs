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
    //public Text Reason;
    private string _uservalue;
    private string _userdigit;
    private int _userstate;
    private int _ansstate = -1;
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
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
