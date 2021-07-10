/************************************************************************************
    作者：荆煦添
    描述：公式编辑器自定义方块显示数据
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using UnityEngine.UI;

public class FormulaCustomizeShower : HTBehaviour
{
    [SerializeField]
    private Text Value;

    public void SetValue(string value)
    {
        Value.text = value;
    }
}
