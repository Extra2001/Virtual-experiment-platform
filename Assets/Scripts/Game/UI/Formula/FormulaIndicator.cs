/************************************************************************************
    作者：荆煦添
    描述：公式编辑器提示框显示器
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class FormulaIndicator : HTBehaviour
{
    public Text Title;
    public Text Desc;
    public Text Value;
    /// <summary>
    /// 显示方块属性
    /// </summary>
    public void ShowIndicate(string title, string desc, string value)
    {
        Title.text = title;
        Desc.text = desc;
        Value.text = value;
        gameObject.rectTransform().SetFloat();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
