/************************************************************************************
    作者：荆煦添
    描述：数据记录表格的单元格
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;
using System;

public class DataInput : HTBehaviour
{
    public Text _GroupNumber;
    public InputField _Value;
    public QuantityModel quantity;
    public bool Inputable
    {
        get => _Value.readOnly;
        set => _Value.readOnly = value;
    }
    public string Value { get => _Value.text; set => _Value.text = value; }
    public int GroupNumber { get => Convert.ToInt32(_GroupNumber.text.Remove(_GroupNumber.text.Length - 1, 1));
        set => _GroupNumber.text = $"{value}."; }

    private void Start()
    {
        _Value.onEndEdit.AddListener(CheckInput);
    }

    public void CheckInput(string input)
    {
        var instrument1 = quantity.InstrumentType.CreateInstrumentInstance();
        var (res1, answer1) = instrument1.CheckErrorLimit(input);
        var (res2, answer2) = instrument1.CheckULLimit(input);
        var instrument2 = GameManager.Instance.CurrentInstrument;
        if (instrument2 == null) instrument2 = instrument1;
        var (res3, answer3) = instrument2.CheckErrorLimit(input);
        var (res4, answer4) = instrument2.CheckULLimit(input);
        if ((res1 && res2) || (res3 && res4)) return;
        if (res1 || res2)
        {
            if (res1)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Title = new BindableString("读数错误"),
                    Message = new BindableString($"你的读数超出仪器的量程，请读入适合仪器：\"{instrument1.InstName}\"的数据。")
                });
            }
            else
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Title = new BindableString("读数错误"),
                    Message = new BindableString($"你的读数精度不正确，请读入适合仪器：\"{instrument1.InstName}\"的数据。\n\n例：{answer1}")
                });
            }
        }
        else if (res3 || res4)
        {
            if (res1)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Title = new BindableString("读数错误"),
                    Message = new BindableString($"你的读数超出仪器的量程，请读入适合仪器：\"{instrument2.InstName}\"的数据。")
                });
            }
            else
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Title = new BindableString("读数错误"),
                    Message = new BindableString($"你的读数精度不正确，请读入适合仪器：\"{instrument2.InstName}\"的数据。\n\n例：{answer3}")
                });
            }
        }
        else
        {
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                ShowCancel = false,
                Title = new BindableString("读数错误"),
                Message = new BindableString($"你的读数不正确，无法将读入的数据与已选择的任何仪器相匹配。")
            });
        }
        RecordManager.tempRecord.score.DataRecordError++;
    }

    /// <summary>
    /// 显示数值
    /// </summary>
    public void Show(QuantityModel quantity, int groupNumber)
    {
        GroupNumber = groupNumber;
        this.quantity = quantity;
    }
    /// <summary>
    /// 显示组数
    /// </summary>
    public void Show(QuantityModel quantity, int groupNumber, string value)
    {
        Value = value;
        GroupNumber = groupNumber;
        this.quantity = quantity;
    }
}
