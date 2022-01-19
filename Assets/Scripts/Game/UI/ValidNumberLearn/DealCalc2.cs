using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MultiPlayAndDivideFormat
{
    public int Sign = 0;//0代表乘法，1代表除法
    public string Value = "0";
    public string Digit = "0";//科学计数法10的几次方
}
public class DealCalc2 : HTBehaviour
{
    public Button CalcButton;
    public Button AddButton;
    public Button SubstractButton;
    public GameObject Cell;
    public Transform CellFather;
    public Text Ans;

    public List<GameObject> Cells;
    public List<MultiPlayAndDivideFormat> CellValue = new List<MultiPlayAndDivideFormat>();


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
        Cells[0].GetComponent<MultiplayAndDivideCell>().id = 0;
        Cells[0].GetComponent<MultiplayAndDivideCell>().Root = this.gameObject;
        CellValue.Add(new MultiPlayAndDivideFormat());
        Cells[1].GetComponent<MultiplayAndDivideCell>().id = 1;
        Cells[1].GetComponent<MultiplayAndDivideCell>().Root = this.gameObject;
        CellValue.Add(new MultiPlayAndDivideFormat());

        //后面代码添加的块初始化
        AddButton.onClick.AddListener(() =>
        {
            var temp = GameObject.Instantiate(Cell);
            temp.transform.parent = CellFather;
            Cells.Add(temp);
            temp.GetComponent<MultiplayAndDivideCell>().id = Cells.Count - 1;
            temp.GetComponent<MultiplayAndDivideCell>().Root = this.gameObject;
            CellValue.Add(new MultiPlayAndDivideFormat());
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
                    Message = "乘除运算至少应当有两个元素",
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
                var temp = item.GetComponent <MultiplayAndDivideCell>();
                finish = finish && temp.IfFinish();
            }
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
                WarningInput();
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
        });

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