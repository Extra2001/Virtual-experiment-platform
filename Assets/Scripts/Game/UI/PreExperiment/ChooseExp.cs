/************************************************************************************
    作者：荆煦添
    描述：选择实验处理程序
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
