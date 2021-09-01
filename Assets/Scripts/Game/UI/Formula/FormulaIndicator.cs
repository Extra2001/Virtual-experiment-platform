/************************************************************************************
    ���ߣ�������
    ��������ʽ�༭����ʾ����ʾ��
*************************************************************************************/
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
        StartCoroutine(CommonTools.DelayGet(gameObject.rectTransform().SetFloat));
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
