/************************************************************************************
    ���ߣ�������
    ��������ʽ�༭����ʾ����ʾ��
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
    /// ��ʾ��������
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
