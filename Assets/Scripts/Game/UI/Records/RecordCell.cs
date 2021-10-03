/************************************************************************************
    ���ߣ�������
    ��������ȡ�浵��Ԫ��
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class RecordCell : HTBehaviour
{
    public Text _title;
    public Text _time;
    public Button _LoadButton;
    public Button _DeleteButton;
    public Button _ShareButton;
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
            _DeleteButton.onClick.AddListener(() =>
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Message = "ȷʵҪɾ���ô浵��",
                    ConfirmAction = () => RecordManager.records[recordId].Delete()
                });
            });
        if (_ShareButton != null)
            _ShareButton.onClick.AddListener(() => { SaveOpenRecord.ExportRecord(RecordManager.records[recordId]); });
    }

    public void SetRecordInfo(RecordInfo recordInfo)
    {
        this.recordId = recordInfo.id;
        this.time = recordInfo.timeString;
        this.title = recordInfo.title;
    }
}