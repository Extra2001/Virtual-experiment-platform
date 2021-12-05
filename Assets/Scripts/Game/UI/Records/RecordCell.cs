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
    public Button _ShareButton;
    public Button _UploadButton;
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
                GameLaunch.Instance.ShowGeneralLoadingScreen();
                RecordManager.GetRecord(recordId, x =>
                {
                    x.Load();
                    GameLaunch.Instance.HideGeneralLoadingScreen();
                }, x =>
                {
                    GameLaunch.Instance.HideGeneralLoadingScreen();
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        ShowCancel = false,
                        Title = "错误",
                        Message = x
                    });
                });
            });
        if (_DeleteButton != null)
            _DeleteButton.onClick.AddListener(() =>
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Message = "确实要删除该存档吗？",
                    ConfirmAction = () =>
                    {
                        GameLaunch.Instance.ShowGeneralLoadingScreen();
                        RecordManager.DeleteRecord(recordId, () => GameLaunch.Instance.HideGeneralLoadingScreen(), () =>
                        {
                            GameLaunch.Instance.HideGeneralLoadingScreen();
                            UIAPI.Instance.ShowModel(new SimpleModel()
                            {
                                ShowCancel = false,
                                Title = "错误",
                                Message = "未能删除该存档"
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
                    GameLaunch.Instance.HideGeneralLoadingScreen();
                    SaveOpenRecord.ExportRecord(x);
                }, x =>
                {
                    GameLaunch.Instance.HideGeneralLoadingScreen();
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        ShowCancel = false,
                        Title = "错误",
                        Message = x
                    });
                });
            });
        }
        if (_UploadButton != null)
        {
            _UploadButton.onClick.AddListener(() =>
            {
                RecordManager.GetRecord(recordId, x =>
                {                                        
                    if (!x.experimentFinish)
                    {
                        UIAPI.Instance.ShowModel(new SimpleModel()
                        {
                            ShowCancel = false,
                            Title = "警告",
                            Message = "该存档实验未完成"
                        });
                    }
                    else
                    {
                        UIAPI.Instance.ShowModel(new SimpleModel()
                        {
                            Title = "提示",
                            Message = "是否要上传pdf",
                            ConfirmAction = () => 
                            {
                                GetComponent<LoadImgFromFile>().OnUploadCustomImgBtnClick();
                            },
                            CancelAction = () =>
                            {
                                GetComponent<LoadImgFromFile>().RecivePng(null);
                            }
                        });
                        
                    }
                    GameLaunch.Instance.HideGeneralLoadingScreen();
                }, x =>
                {
                    GameLaunch.Instance.HideGeneralLoadingScreen();
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        ShowCancel = false,
                        Title = "错误",
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