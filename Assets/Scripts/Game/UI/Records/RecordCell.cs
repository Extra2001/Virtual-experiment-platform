/************************************************************************************
    作者：荆煦添
    描述：读取存档单元格
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class RecordCell : HTBehaviour
{
    public Text _title;
    public Text _time;
    public Button _LoadButton;
    public Button _DeleteButton;
    public LoadRecord _LoadRecord;
    public int recordId { get; set; }
    public string title
    {
        get => _title.text;
        set => _title.text = value;
    }
    public string time
    {
        get => _time.text;
        set => _time.text = value;
    }

    void Start()
    {
        if (_LoadButton != null)
            _LoadButton.onClick.AddListener(() => { Main.m_UI.GetOpenedUI<ReadRecordUILogic>()?.NavigateBack(); RecordManager.records[recordId].Load(); });
        if (_DeleteButton != null)
            _DeleteButton.onClick.AddListener(() => { RecordManager.records[recordId].Delete(); });
    }

    public void SetRecordInfo(RecordInfo recordInfo)
    {
        this.recordId = recordInfo.id;
        this.time = recordInfo.timeString;
        this.title = recordInfo.title;
    }
}