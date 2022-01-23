using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionCalcCell1 : HTBehaviour
{
    public InputField Value;
    public InputField Value2;
    public InputField Digit;
    public Button SwitchButton;

    public GameObject root;
    public int id;
    public int state = 0;//0代表正常数，1代表科学计数法
    public bool[] FinishSituation = new bool[3] { false, false, false }; //0代表正常数，1和2代表科学计数法
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        Value.onEndEdit.AddListener(value =>
        {
            if ((!string.IsNullOrEmpty(value)) && double.TryParse(value, out double t))
            {
                if (value.Length > 9)
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Title = "警告",
                        Message = "输入的数字太精确了，本模块无法计算",
                        ShowCancel = false
                    });
                    Value.text = string.Empty;
                    return;
                }
                if ((double.Parse(value) - 1 >= 0) && (double.Parse(value) - 10 < 0))//对数特殊要求
                {
                    root.GetComponent<DealCalc3>().CellValue[id].Value = value;
                    FinishSituation[1] = true;
                    return;
                }
                else
                {
                    Value.text = string.Empty;
                    WarningInput();
                }                
            }
            FinishSituation[1] = false;
        });
        Digit.onEndEdit.AddListener(value =>
        {
            if ((!string.IsNullOrEmpty(value)) && double.TryParse(value, out double t))
            {
                if (Mathf.Abs(int.Parse(value)) > 9)
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Title = "警告",
                        Message = "输入的数字太精确了，本模块无法计算",
                        ShowCancel = false
                    });
                    Digit.text = string.Empty;
                    return;
                }
                if (int.Parse(value) != 0)
                {
                    root.GetComponent<DealCalc3>().CellValue[id].Digit = value;
                    FinishSituation[2] = true;
                    return;
                }
                else
                {
                    Digit.text = string.Empty;
                    WarningInput();
                }
            }
            FinishSituation[2] = false;
        });
        Value2.onEndEdit.AddListener(value =>
        {
            if ((!string.IsNullOrEmpty(value)) && double.TryParse(value, out double t))
            {
                if (value.Length > 9)
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        Title = "警告",
                        Message = "输入的数字太精确了，本模块无法计算",
                        ShowCancel = false
                    });
                    Value2.text = string.Empty;
                    return;
                }
                if ((double.Parse(value) - 0) > 0)//对数特殊要求
                {
                    root.GetComponent<DealCalc3>().CellValue[id].Value = value;
                    root.GetComponent<DealCalc3>().CellValue[id].Digit = "0";
                    FinishSituation[0] = true;
                    return;
                }
                else
                {
                    Value2.text = string.Empty;
                    WarningInput();
                }
            }
            FinishSituation[0] = false;
        });

        SwitchButton.onClick.AddListener(() =>
        {
            Value.text = string.Empty;
            Digit.text = string.Empty;
            Value2.text = string.Empty;
            FinishSituation = new bool[3] { false, false, false };

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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void WarningInput()
    {
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Title = "警告",
            Message = "请输入合法的数字",
            ShowCancel = false
        });
    }
    public bool IfFinish()
    {
        bool ans = true;
        if (state == 0)
        {
            ans = ans && FinishSituation[0];
        }
        else
        {
            ans = ans && FinishSituation[1] && FinishSituation[2];
        }
        return ans;
    }
}
