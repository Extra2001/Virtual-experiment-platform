/************************************************************************************
    作者：荆煦添
    描述：数据记录表格的单元格
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;
using System;
using System.Linq;

public class DataInput : HTBehaviour
{
    public Text _GroupNumber;
    public InputField _Value;
    public Button _DeleteButton;

    public DataColumnModel dataColumnModel;
    [NonSerialized]
    public DataColumn dataColumn;
    public int Index
    {
        get => index; set
        {
            index = value;
            _GroupNumber.text = $"{value + 1}.";
        }
    }
    public bool ReadOnly
    {
        get => _Value.readOnly;
        set => _Value.readOnly = value;
    }
    public string Value
    {
        get => _Value.text;
        set
        {
            dataColumnModel.data[index] = value;
            _Value.text = value;
        }
    }
    public bool Deletable
    {
        get => _DeleteButton.interactable;
        set => _DeleteButton.interactable = value;
    }
    private int index;

    private void Start()
    {
        _Value.onEndEdit.AddListener(CheckInput);
        _DeleteButton.onClick.AddListener(() => dataColumn.DeleteInput(this));
    }

    public void CheckInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return;
        if (ReadOnly) return;
        dataColumnModel.data[Index] = input;
        if (dataColumnModel.type == DataColumnType.Mesured || dataColumnModel.type == DataColumnType.Differenced)
            CheckInstrument(input);
    }

    private void CheckInstrument(string input)
    {
        var quantity = RecordManager.tempRecord.quantities.Where(x => x.Symbol.Equals(dataColumnModel.quantitySymbol)).FirstOrDefault();
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
    public void Show(DataColumnModel dataColumnModel, int index, string value = null)
    {
        Index = index;
        this.dataColumnModel = dataColumnModel;

        if (!string.IsNullOrEmpty(value))
            Value = value;
        else if (!string.IsNullOrEmpty(dataColumnModel.data[Index]))
            Value = dataColumnModel.data[Index];
    }
}
