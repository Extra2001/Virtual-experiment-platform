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
    public bool Inputable
    {
        get => _Value.readOnly;
        set => _Value.readOnly = value;
    }
    public string Value { get => _Value.text; set => _Value.text = value; }
    public int GroupNumber { get => Convert.ToInt32(_GroupNumber.text.Remove(_GroupNumber.text.Length - 1, 1)); 
        set => _GroupNumber.text = $"{value}."; }
    /// <summary>
    /// 显示数值
    /// </summary>
    public void Show(int groupNumber)
    {
        GroupNumber = groupNumber;
    }
    /// <summary>
    /// 显示组数
    /// </summary>
    public void Show(int groupNumber, string value)
    {
        Value = value;
        GroupNumber = groupNumber;
    }
}
