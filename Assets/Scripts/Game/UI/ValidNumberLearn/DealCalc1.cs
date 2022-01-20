using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AddAndSubstractFormat
{
    public int Sign = 0;//0代表加法，1代表减法
    public string Value = "0";
    public string Digit = "0";//科学计数法10的几次方
}
public class DealCalc1 : HTBehaviour
{
    public Button CalcButton;
    public Button AddButton;
    public Button SubstractButton;
    public GameObject Cell;
    public Transform CellFather;
    public Text Ans;

    public List<GameObject> Cells;
    public List<AddAndSubstractFormat> CellValue = new List<AddAndSubstractFormat>();


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

    public bool IfMix;
    public GameObject MixControlObject;
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        Reason.text = "";
        //加法前两个先初始化
        Cells[0].GetComponent<AddAndSubtractCell>().id = 0;
        Cells[0].GetComponent<AddAndSubtractCell>().Root = this.gameObject;
        CellValue.Add(new AddAndSubstractFormat());
        Cells[1].GetComponent<AddAndSubtractCell>().id = 1;
        Cells[1].GetComponent<AddAndSubtractCell>().Root = this.gameObject;
        CellValue.Add(new AddAndSubstractFormat());

        //后面代码添加的块初始化
        AddButton.onClick.AddListener(() =>
        {
            var temp = GameObject.Instantiate(Cell);
            temp.transform.parent = CellFather;
            Cells.Add(temp);
            temp.GetComponent<AddAndSubtractCell>().id = Cells.Count - 1;
            temp.GetComponent<AddAndSubtractCell>().Root = this.gameObject;
            CellValue.Add(new AddAndSubstractFormat());            
        });

        SubstractButton.onClick.AddListener(() =>
        {
            if (Cells.Count > 2)
            {
                var temp = Cells[Cells.Count - 1];
                Cells.RemoveAt(Cells.Count - 1);
                CellValue.RemoveAt(CellValue.Count - 1);
                DestroyImmediate(temp);
            }
            else
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Message = "加减运算至少应当有两个元素",
                    ShowCancel = false
                });
            }            
        });
        CalcButton.onClick.AddListener(() =>
        {
            bool finish = true;
            //检查算式
            foreach (var item in Cells)
            {
                var temp = item.GetComponent<AddAndSubtractCell>();
                finish = finish && temp.IfFinish();           
            }
            //检查答案输入
            if (!IfMix)
            {
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
            }
            

            if (!finish)
            {
                WarningInput();
            }
            else if (!IfMix)//非混合运算
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
                (bool correct, string message, CheckFloat2 correctvalue) = CheckFloat2.CheckGroupAdd(input, userresult);

                if (correctvalue.ToString().Contains("E") || correctvalue.ToString().Contains("e"))
                {
                    _ansstate = 1;
                    Ans.text = StaticMethods.ExpToSci(correctvalue.ToString());
                }
                else
                {
                    _ansstate = 0;
                    Ans.text = correctvalue.ToString();
                }
                if (correct)
                {
                    Reason.text = "计算正确";
                }
                else
                {
                    Reason.text = message;
                }
            }
            else if(IfMix)//混合运算
            {
                List<(CheckFloat2 rawnumstr, bool isadd)> input = new List<(CheckFloat2 rawnumstr, bool isadd)>();
                (CheckFloat2 rawnumstr, bool isadd) temp;
                for (int i = 0; i < CellValue.Count; i++)
                {
                    if (CellValue[i].Sign == 0)
                    {
                        temp.isadd = true;
                    }
                    else
                    {
                        temp.isadd = false;
                    }
                    string temp1 = StaticMethods.SciToExp(CellValue[i].Value + "*10^(" + CellValue[i].Digit + ")");
                    temp.rawnumstr = new CheckFloat2(temp1);
                    input.Add(temp);
                }
                string userresult = StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")");
                CheckFloat2 CorrectValue;//正确保留后的值
                decimal RealValue;//未保留的值
                string ShowValue;//此处显示的值
                CorrectValue = CheckFloat2.GroupAdd(input);
                RealValue = CheckFloat2.GroupAddRaw(input);
                int temp2 = MixControlObject.GetComponent<MixCalcControl>().NumRealLength;
                if (temp2 > CorrectValue.EffectiveDigit || temp2 == 0)
                {
                    temp2 = CorrectValue.EffectiveDigit;
                    MixControlObject.GetComponent<MixCalcControl>().NumRealLength = temp2;
                }
                temp2 = CorrectValue.EffectiveDigit + 1;//显示值比真实结果多保留一位
                ShowValue = new CheckFloat2(CheckFloat2.KeepEffective(RealValue, temp2)).ToString();
                MixControlObject.GetComponent<MixCalcControl>().LastValue = ShowValue;
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
                CheckFloat2 _user = new CheckFloat2(userresult);
                bool correct = (_ans.EffectiveDigit == _user.EffectiveDigit) && (_ans.Value - _user.Value == 0);

                if (correct)
                {
                    Reason.text = "计算正确";
                }
                else
                {
                    Reason.text = "计算错误";
                }
            }
        });

        //用户答案输入初始化
        UserValue.onValueChanged.AddListener(value =>
        {
            if ((!string.IsNullOrEmpty(value)) && double.TryParse(value, out double t))
            {
                if (value.Length > 10)
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Title = "警告",
                        Message = "输入的数字太精确了，本模块无法计算",
                        ShowCancel = false
                    });
                    UserValue.text = string.Empty;
                    return;
                }
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
                if (Mathf.Abs(int.Parse(value)) > 10)
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Title = "警告",
                        Message = "输入的数字太精确了，本模块无法计算",
                        ShowCancel = false
                    });
                    UserDigit.text = string.Empty;
                    return;
                }
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
                if (value.Length > 10)
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Title = "警告",
                        Message = "输入的数字太精确了，本模块无法计算",
                        ShowCancel = false
                    });
                    UserValue2.text = string.Empty;
                    return;
                }
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
                        AnsSwitchButton.FindChildren("Text").GetComponent<Text>().text = @"a*10^(b) <=> a";
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
                        AnsSwitchButton.FindChildren("Text").GetComponent<Text>().text = @"a <=> a*10^(b)";
                        _ansstate = 0;
                    }
                }
            }
        });



    }

    void Update()
    {
        
    }
    private void WarningInput()
    {
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Title = "警告",
            Message = "请检查算式及结果是否输入完成",
            ShowCancel = false
        });
    }
}
