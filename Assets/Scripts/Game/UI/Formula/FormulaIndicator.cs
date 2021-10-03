/************************************************************************************
    作者：荆煦添
    描述：公式编辑器提示框显示器
*************************************************************************************/
using DG.Tweening;
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
        gameObject.SetActive(true);
        gameObject.SetFloatWithAnimation(this);
    }

    public void Hide()
    {
        gameObject.transform.DOScale(0, 0.3f)
            .SetEase(Ease.OutExpo);
        Invoke(nameof(Close), 0.3f);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
