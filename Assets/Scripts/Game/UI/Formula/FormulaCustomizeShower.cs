/************************************************************************************
    ���ߣ�������
    ��������ʽ�༭���Զ��巽����ʾ����
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
