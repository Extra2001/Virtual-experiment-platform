/************************************************************************************
    ���ߣ�������
    ������ѡ��ʵ�鴦�����
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class ChooseExp : HTBehaviour
{
    public string expName;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Initializer.GetPresetRecord(expName).Load();
        });
    }
}
