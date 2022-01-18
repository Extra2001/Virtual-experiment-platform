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
            if (!string.IsNullOrEmpty(value))
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
            if (!string.IsNullOrEmpty(value))
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
            if (!string.IsNullOrEmpty(value))
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

        //CalcButton绑定
        CalcButton.onClick.AddListener(() =>
        {
            //检查算式输完没有
            if (CurrentFunctionIndex == -1)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Message = "请在您想使用的对应函数前打勾",
                    ShowCancel = false
                });
                return;
            }else if (CurrentFunctionIndex < 2)
            {
                if (!Cells[CurrentFunctionIndex].GetComponent<FunctionCalcCell1>().IfFinish())
                {
                    WarningInput();
                    return;
                }
            }
            else if (CurrentFunctionIndex < 5)
            {
                if (!Cells[CurrentFunctionIndex].GetComponent<FunctionCalcCell2>().IfFinish())
                {
                    WarningInput();
                    return;
                }
            }
            else
            {
                if (!Cells[CurrentFunctionIndex].GetComponent<FunctionCalcCell3>().IfFinish())
                {
                    WarningInput();
                    return;
                }
            }
            //检查答案输完没有
            if (_userstate == 0)
            {
                if (string.IsNullOrEmpty(UserValue2.text))
                {
                    WarningInput();
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(UserValue.text) || string.IsNullOrEmpty(UserDigit.text))
                {
                    WarningInput();
                    return;
                }
            }

            //计算
            CheckFloat2 result = new CheckFloat2();
            bool correct = false;
            switch (CurrentFunctionIndex)
            {
                case 0:
                    (result, correct) = CheckFloat2.CheckUserLog(Math.E, StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                    break;
                case 1:
                    (result, correct) = CheckFloat2.CheckUserLog(10.0, StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                    break;
                case 2:
                    (result, correct) = CheckFloat2.CheckUserExp(double.Parse(CellValue[CurrentFunctionIndex].A), StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                    break;
                case 3:
                    (result, correct) = CheckFloat2.CheckUserPow(double.Parse(CellValue[CurrentFunctionIndex].A), StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                    break;
                case 4:
                    (result, correct) = CheckFloat2.CheckUserPow(double.Parse(CellValue[CurrentFunctionIndex].A), StaticMethods.SciToExp(CellValue[CurrentFunctionIndex].Value + "*10^(" + CellValue[CurrentFunctionIndex].Digit + ")"), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
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
                        (result, correct) = CheckFloat2.CheckUserSin(angle[0], angle[1], angle[2], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
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
                        (result, correct) = CheckFloat2.CheckUserSin(angle[0], angle[1], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                    }
                    else
                    {
                        (result, correct) = CheckFloat2.CheckUserSin(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
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
                        (result, correct) = CheckFloat2.CheckUserCos(angle[0], angle[1], angle[2], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
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
                        (result, correct) = CheckFloat2.CheckUserCos(angle[0], angle[1], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                    }
                    else
                    {
                        (result, correct) = CheckFloat2.CheckUserCos(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
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
                        (result, correct) = CheckFloat2.CheckUserTan(angle[0], angle[1], angle[2], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
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
                        (result, correct) = CheckFloat2.CheckUserTan(angle[0], angle[1], StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                    }
                    else
                    {
                        (result, correct) = CheckFloat2.CheckUserTan(int.Parse(CellValue[CurrentFunctionIndex].Angle[0]), StaticMethods.SciToExp(_uservalue + "*10^(" + _userdigit + ")"));
                    }
                    break;
                default:
                    break;
            }
            if (result.ToString().Contains("E") || result.ToString().Contains("e"))
            {
                _ansstate = 1;
                Ans.text = StaticMethods.ExpToSci(result.ToString());
            }
            else
            {
                _ansstate = 0;
                Ans.text = result.ToString();
            }
            if (correct)
            {
                Reason.text = "计算正确";
            }
            else
            {
                Reason.text = "计算错误";
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
