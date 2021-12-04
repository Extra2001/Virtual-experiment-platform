/************************************************************************************
    ×÷Õß£º¾£ìãÌí
    ÃèÊö£º¶ÁÈ¡´æµµµ¥Ôª¸ñ
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
            _LoadButton.onClick.AddListener(() =>
            {
                Main.m_UI.NavigateBackTemporaryUI();
                UIAPI.Instance.ShowLoading();
                RecordManager.GetRecord(recordId, x =>
                {
                    x.Load();
                    UIAPI.Instance.HideLoading();
                }, x =>
                {
                    UIAPI.Instance.HideLoading();
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        ShowCancel = false,
                        Title = "´íÎó",
                        Message = x
                    });
                });
            });
        if (_DeleteButton != null)
            _DeleteButton.onClick.AddListener(() =>
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Message = "È·ÊµÒªÉ¾³ý¸Ã´æµµÂð£¿",
                    ConfirmAction = () =>
                    {
                        UIAPI.Instance.ShowLoading();
                        RecordManager.DeleteRecord(recordId, () => UIAPI.Instance.HideLoading(), () =>
                        {
                            UIAPI.Instance.HideLoading();
                            UIAPI.Instance.ShowModel(new SimpleModel()
                            {
                                ShowCancel = false,
                                Title = "´íÎó",
                                Message = "Î´ÄÜÉ¾³ý¸Ã´æµµ"
                            });
                        });
                    }
                });
            });
        if (_ShareButton != null)
        {
            _ShareButton.onClick.AddListener(() =>
            {
                RecordManager.GetRecord(recordId, x =>
                {
                    UIAPI.Instance.HideLoading();
                    SaveOpenRecord.ExportRecord(x);
                }, x =>
                {
                    UIAPI.Instance.HideLoading();
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        ShowCancel = false,
                        Title = "´íÎó",
                        Message = x
                    });
                });
            });
        }
    }

    public void SetRecordInfo(RecordInfo recordInfo)
    {
        this.recordId = recordInfo.id;
        this.time = recordInfo.timeString;
        this.title = recordInfo.title;
    }
}