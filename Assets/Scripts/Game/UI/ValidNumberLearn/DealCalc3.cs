using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    public bool IfMix;
    public GameObject MixControlObject;
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

        //CalcButton绑定
        CalcButton.onClick.AddListener(() =>
        {
            try
            {
                bool finish = true;
                //检查算式输完没有
                if (CurrentFunctionIndex == -1)
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Message = "请在您想使用的对应函数前打勾",
                        ShowCancel = false
                    });
                    finish = false;
                }
                else if (CurrentFunctionIndex < 2)
                {
                    if (!Cells[CurrentFunctionIndex].GetComponent<FunctionCalcCell1>().IfFinish())
                    {
                        WarningInput();
                        finish = false;
                    }
                }
                else if (CurrentFunctionIndex < 5)
                {
                    if (!Cells[CurrentFunctionIndex].GetComponent<FunctionCalcCell2>().IfFinish())
                    {
                        WarningInput();
                        finish = false;
                    }
                }
                else
                {
                    if (!Cells[CurrentFunctionIndex].GetComponent<FunctionCalcCell3>().IfFinish())
                    {
                        WarningInput();
                        finish = false;
                    }
                }
                //检查答案输完没有
                if (!IfMix)
                {
                    if (_userstate == 0)
                    {
                        if (string.IsNullOrEmpty(UserValue2.text))
                        {
                            WarningInput();
                            finish = false;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(UserValue.text) || string.IsNullOrEmpty(UserDigit.text))
                        {
                            WarningInput();
                            finish = false;
                        }
                    }
                }
                if (!finish)
                {
                    Ans.text = "?";
                    return;
                }

                //计算
                CheckFloat2 result = new CheckFloat2();
                bool correct = false;
                string userresult = StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")");
                CheckFloat2 HiddenValue = new CheckFloat2();//用于传递有效位数的值
                double RealValue = 0;//未保留的值
                string ShowValue;//此处显示的值
                switch (CurrentFunctionIndex)
                {
                    case 0:
                        if (!IfMix)
                        {
                            (result, correct) = CheckFloat2.CheckUserLog(Math.E, StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                        }
                        else
                        {
                            string input = StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")");
                            CheckFloat2 temp = new CheckFloat2(input);
                            RealValue = CheckFloat2.LogRaw(Math.E, temp);
                            temp = StaticMethods.CheckSimilar(temp, MixControlObject.GetComponent<MixCalcControl>().HistoryResult);
                            HiddenValue = CheckFloat2.Log(Math.E, temp);
                        }
                        break;
                    case 1:
                        if (!IfMix)
                        {
                            (result, correct) = CheckFloat2.CheckUserLog(10.0, StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                        }
                        else
                        {
                            string input = StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")");
                            CheckFloat2 temp = new CheckFloat2(input);
                            RealValue = CheckFloat2.LogRaw(10.0, temp);
                            temp = StaticMethods.CheckSimilar(temp, MixControlObject.GetComponent<MixCalcControl>().HistoryResult);
                            HiddenValue = CheckFloat2.Log(10.0, temp);
                        }
                        break;
                    case 2:
                        if (!IfMix)
                        {
                            (result, correct) = CheckFloat2.CheckUserExp(double.Parse(CellValue[CurrentFunctionIndex].A), StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                        }
                        else
                        {
                            string input = StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")");
                            CheckFloat2 temp = new CheckFloat2(input);
                            RealValue = CheckFloat2.ExpRaw(double.Parse(CellValue[CurrentFunctionIndex].A), temp);
                            temp = StaticMethods.CheckSimilar(temp, MixControlObject.GetComponent<MixCalcControl>().HistoryResult);
                            HiddenValue = CheckFloat2.Exp(double.Parse(CellValue[CurrentFunctionIndex].A), temp);
                        }
                        break;
                    case 3:
                        if (!IfMix)
                        {
                            (result, correct) = CheckFloat2.CheckUserPow(double.Parse(CellValue[CurrentFunctionIndex].A), StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                        }
                        else
                        {
                            string input = StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")");
                            CheckFloat2 temp = new CheckFloat2(input);
                            RealValue = CheckFloat2.PowRaw(temp, double.Parse(CellValue[CurrentFunctionIndex].A));
                            temp = StaticMethods.CheckSimilar(temp, MixControlObject.GetComponent<MixCalcControl>().HistoryResult);
                            HiddenValue = CheckFloat2.Pow(temp, double.Parse(CellValue[CurrentFunctionIndex].A));
                        }
                        break;
                    case 4:
                        if (!IfMix)
                        {
                            (result, correct) = CheckFloat2.CheckUserPow(double.Parse(CellValue[CurrentFunctionIndex].A), StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                        }
                        else
                        {
                            string input = StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")");
                            CheckFloat2 temp = new CheckFloat2(input);
                            RealValue = CheckFloat2.PowRaw(temp, double.Parse(CellValue[CurrentFunctionIndex].A));
                            temp = StaticMethods.CheckSimilar(temp, MixControlObject.GetComponent<MixCalcControl>().HistoryResult);
                            HiddenValue = CheckFloat2.Pow(temp, double.Parse(CellValue[CurrentFunctionIndex].A));
                        }
                        break;
                    case 5:
                        if (CellValue[CurrentFunctionIndex].AngleKind[2])
                        {
                            int[] angle = new int[3];
                            for (int i = 0; i < 3; i++)
                            {
                                if (string.IsNullOrEmpty(CellValue[CurrentFunctionIndex].Angle[i]))
                                {
                                    angle[i] = 0;
                                }
                                else
                                {
                                    angle[i] = int.Parse(CellValue[CurrentFunctionIndex].Angle[i]);
                                }
                            }
                            if (!IfMix)
                            {
                                (result, correct) = CheckFloat2.CheckUserSin(angle[0], angle[1], angle[2], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                            }
                            else
                            {
                                double val;
                                double unc;
                                (val, unc) = CheckFloat2.MakeRadian(angle[0], angle[1], angle[2]);
                                HiddenValue = CheckFloat2.MySin(val, unc);
                                RealValue = CheckFloat2.MySinRaw(angle[0], angle[1], angle[2]);
                            }
                        }
                        else if (CellValue[CurrentFunctionIndex].AngleKind[1])
                        {
                            int[] angle = new int[2];
                            for (int i = 0; i < 2; i++)
                            {
                                if (string.IsNullOrEmpty(CellValue[CurrentFunctionIndex].Angle[i]))
                                {
                                    angle[i] = 0;
                                }
                                else
                                {
                                    angle[i] = int.Parse(CellValue[CurrentFunctionIndex].Angle[i]);
                                }
                            }
                            if (!IfMix)
                            {
                                (result, correct) = CheckFloat2.CheckUserSin(angle[0], angle[1], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                            }
                            else
                            {
                                double val;
                                double unc;
                                (val, unc) = CheckFloat2.MakeRadian(angle[0], angle[1]);
                                HiddenValue = CheckFloat2.MySin(val, unc);
                                RealValue = CheckFloat2.MySinRaw(angle[0], angle[1]);
                            }
                        }
                        else
                        {
                            if (!IfMix)
                            {
                                (result, correct) = CheckFloat2.CheckUserSin(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                            }
                            else
                            {
                                double val;
                                double unc;
                                (val, unc) = CheckFloat2.MakeRadian(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]));
                                HiddenValue = CheckFloat2.MySin(val, unc);
                                RealValue = CheckFloat2.MySinRaw(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]));
                            }
                        }
                        break;
                    case 6:
                        if (CellValue[CurrentFunctionIndex].AngleKind[2])
                        {
                            int[] angle = new int[3];
                            for (int i = 0; i < 3; i++)
                            {
                                if (string.IsNullOrEmpty(CellValue[CurrentFunctionIndex].Angle[i]))
                                {
                                    angle[i] = 0;
                                }
                                else
                                {
                                    angle[i] = int.Parse(CellValue[CurrentFunctionIndex].Angle[i]);
                                }
                            }
                            if (!IfMix)
                            {
                                (result, correct) = CheckFloat2.CheckUserCos(angle[0], angle[1], angle[2], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                            }
                            else
                            {
                                double val;
                                double unc;
                                (val, unc) = CheckFloat2.MakeRadian(angle[0], angle[1], angle[2]);
                                HiddenValue = CheckFloat2.MyCos(val, unc);
                                RealValue = CheckFloat2.MyCosRaw(angle[0], angle[1], angle[2]);
                            }
                        }
                        else if (CellValue[CurrentFunctionIndex].AngleKind[1])
                        {
                            int[] angle = new int[2];
                            for (int i = 0; i < 2; i++)
                            {
                                if (string.IsNullOrEmpty(CellValue[CurrentFunctionIndex].Angle[i]))
                                {
                                    angle[i] = 0;
                                }
                                else
                                {
                                    angle[i] = int.Parse(CellValue[CurrentFunctionIndex].Angle[i]);
                                }
                            }
                            if (!IfMix)
                            {
                                (result, correct) = CheckFloat2.CheckUserCos(angle[0], angle[1], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                            }
                            else
                            {
                                double val;
                                double unc;
                                (val, unc) = CheckFloat2.MakeRadian(angle[0], angle[1]);
                                HiddenValue = CheckFloat2.MyCos(val, unc);
                                RealValue = CheckFloat2.MyCosRaw(angle[0], angle[1]);
                            }
                        }
                        else
                        {
                            if (!IfMix)
                            {
                                (result, correct) = CheckFloat2.CheckUserCos(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                            }
                            else
                            {
                                double val;
                                double unc;
                                (val, unc) = CheckFloat2.MakeRadian(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]));
                                HiddenValue = CheckFloat2.MyCos(val, unc);
                                RealValue = CheckFloat2.MyCosRaw(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]));
                            }
                        }
                        break;
                    case 7:
                        if (CellValue[CurrentFunctionIndex].AngleKind[2])
                        {
                            int[] angle = new int[3];
                            for (int i = 0; i < 3; i++)
                            {
                                if (string.IsNullOrEmpty(CellValue[CurrentFunctionIndex].Angle[i]))
                                {
                                    angle[i] = 0;
                                }
                                else
                                {
                                    angle[i] = int.Parse(CellValue[CurrentFunctionIndex].Angle[i]);
                                }
                            }
                            if (!IfMix)
                            {
                                (result, correct) = CheckFloat2.CheckUserTan(angle[0], angle[1], angle[2], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                            }
                            else
                            {
                                double val;
                                double unc;
                                (val, unc) = CheckFloat2.MakeRadian(angle[0], angle[1], angle[2]);
                                HiddenValue = CheckFloat2.MyTan(val, unc);
                                RealValue = CheckFloat2.MyTanRaw(angle[0], angle[1], angle[2]);
                            }
                        }
                        else if (CellValue[CurrentFunctionIndex].AngleKind[1])
                        {
                            int[] angle = new int[2];
                            for (int i = 0; i < 2; i++)
                            {
                                if (string.IsNullOrEmpty(CellValue[CurrentFunctionIndex].Angle[i]))
                                {
                                    angle[i] = 0;
                                }
                                else
                                {
                                    angle[i] = int.Parse(CellValue[CurrentFunctionIndex].Angle[i]);
                                }
                            }
                            if (!IfMix)
                            {
                                (result, correct) = CheckFloat2.CheckUserTan(angle[0], angle[1], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                            }
                            else
                            {
                                double val;
                                double unc;
                                (val, unc) = CheckFloat2.MakeRadian(angle[0], angle[1]);
                                HiddenValue = CheckFloat2.MyTan(val, unc);
                                RealValue = CheckFloat2.MyTanRaw(angle[0], angle[1]);
                            }
                        }
                        else
                        {
                            if (!IfMix)
                            {
                                (result, correct) = CheckFloat2.CheckUserTan(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                            }
                            else
                            {
                                double val;
                                double unc;
                                (val, unc) = CheckFloat2.MakeRadian(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]));
                                HiddenValue = CheckFloat2.MyTan(val, unc);
                                RealValue = CheckFloat2.MyTanRaw(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]));
                            }
                        }
                        break;
                    default:
                        break;
                }

                if (IfMix)
                {
                    ShowValue = new CheckFloat2(CheckFloat2.KeepEffective((decimal)RealValue, 9)).ToString();
                    MixControlObject.GetComponent<MixCalcControl>().HistoryResult.Add(HiddenValue);
                    MixControlObject.GetComponent<MixCalcControl>().NumRealLength = HiddenValue.EffectiveDigit;
                    MixControlObject.GetComponent<MixCalcControl>().LastValue = ShowValue;
                    MixControlObject.GetComponent<MixCalcControl>().RecordNum += 1;
                    result = new CheckFloat2(ShowValue);
                }
                if (result.ToString().Contains("E") || result.ToString().Contains("e"))
                {
                    _ansstate = 1;
                    Ans.text = StaticMethods.ExpToSci(result.ToString());
                    AnsSwitchButton.FindChildren("Text").GetComponent<Text>().text = @"a*10^(b) <=> a";
                }
                else
                {
                    _ansstate = 0;
                    Ans.text = result.ToString();
                    AnsSwitchButton.FindChildren("Text").GetComponent<Text>().text = @"a <=> a*10^(b)";
                }
                if (!IfMix)
                {
                    if (correct)
                    {
                        Reason.text = "计算正确";
                    }
                    else
                    {
                        Reason.text = "计算错误";
                    }
                }
            }
            catch
            {
                Ans.text = "?";
                Reason.text = "计算过程发生异常";
            }                      
        });
        
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
