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
    public Sprite AddImage;
    public Sprite SubtractImage;

    public List<GameObject> Cells;
    private List<AddAndSubstractFormat> CellValue = new List<AddAndSubstractFormat>();
    
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        //加法前两个先初始化
        CellValue.Add(new AddAndSubstractFormat());
        Cells[0].transform.FindChildren("Value").GetComponent<InputField>().onValueChanged.AddListener(value =>
        {
            int i = 0;
            CellValue[i].Value = decimal.Parse(value);
        });
        Cells[0].transform.FindChildren("Digit").GetComponent<InputField>().onValueChanged.AddListener(value =>
        {
            int i = 0;
            CellValue[i].Value = int.Parse(value);
        });
        CellValue.Add(new AddAndSubstractFormat());
        Cells[1].transform.FindChildren("Value").GetComponent<InputField>().onValueChanged.AddListener(value =>
        {
            int i = 1;
            CellValue[i].Value = decimal.Parse(value);
        });
        Cells[1].transform.FindChildren("Digit").GetComponent<InputField>().onValueChanged.AddListener(value =>
        {
            int i = 1;
            CellValue[i].Value = int.Parse(value);
        });
        Cells[1].transform.FindChildren("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            int i = 1;
            CellValue[i].Sign = 1 - CellValue[i].Sign;
            if (CellValue[i].Sign == 0)
            {
                Cells[i].transform.FindChildren("Button").GetComponent<Button>().image.sprite = AddImage;
            }
            else
            {
                Cells[i].transform.FindChildren("Button").GetComponent<Button>().image.sprite = SubtractImage;
            }
        });
        //后面代码添加的块初始化
        AddButton.onClick.AddListener(() =>
        {
            var temp = GameObject.Instantiate(Cell);
            temp.transform.parent = CellFather;
            Cells.Add(temp);
            CellValue.Add(new AddAndSubstractFormat());
            temp.transform.FindChildren("Value").GetComponent<InputField>().onValueChanged.AddListener(value =>
            {
                int i = Cells.Count - 1;
                CellValue[i].Value = decimal.Parse(value);
            });
            temp.transform.FindChildren("Digit").GetComponent<InputField>().onValueChanged.AddListener(value =>
            {
                int i = Cells.Count - 1;
                CellValue[i].Value = int.Parse(value);
            });
            temp.transform.FindChildren("Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                int i = Cells.Count - 1;
                CellValue[i].Sign = 1 - CellValue[i].Sign;
                if (CellValue[i].Sign == 0)
                {
                    Cells[i].transform.FindChildren("Button").GetComponent<Button>().image.sprite = AddImage;
                }
                else
                {
                    Cells[i].transform.FindChildren("Button").GetComponent<Button>().image.sprite = SubtractImage;
                }                
            });

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
