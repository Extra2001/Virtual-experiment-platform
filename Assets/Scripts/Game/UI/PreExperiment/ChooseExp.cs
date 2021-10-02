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
            if (RecordManager.tempRecord.showedInstrument != null
            && RecordManager.tempRecord.showedInstrument.instrumentType != null)
                Main.m_Entity.HideEntity(GameManager.Instance.CurrentInstrument);
            CreateObject.HideCurrent();
            CreateInstrument.HideCurrent();
            RecordManager.ClearTempRecord();
            Initializer.GetPresetRecord(expName).Load();
        });
    }
}
