using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddAndSubstractFormat
{
    public int Sign = 0;//0代表加法，1代表减法
    public decimal Value = 0;
    public int Digit = 0;//科学计数法10的几次方
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
    private decimal _uservalue;
    private int _userdigit;
    private int _userstate;
    private int _ansstate = -1;
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
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
                //弹出报错框
            }            
        });
        CalcButton.onClick.AddListener(() =>
        {
            bool finish = true;
            //检查算式
            foreach (var item in Cells)
            {
                var temp = item.GetComponent<AddAndSubtractCell>();
                if (temp.state == 1)
                {
                    if (temp.Value.text == "0" || temp.Digit.text == "0")
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

            if (!finish)
            {
                Debug.Log("未开始计算");
            }
            else
            {


                _ansstate = 1;
                Debug.Log("开始计算");
            }
        });

        UserValue.onValueChanged.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                UserValue.text = "0";
            }
            else
            {
                _uservalue = decimal.Parse(value);
            }
        });
        UserDigit.onValueChanged.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                UserDigit.text = "0";
            }
            else
            {
                _userdigit = int.Parse(value);
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
                //Root.GetComponent<DealCalc1>().CellValue[id].Value = decimal.Parse(value);
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

    // Update is called once per frame
    void Update()
    {

    }
}
