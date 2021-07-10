/************************************************************************************
    作者：荆煦添
    描述：保存存档单元格
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class SaveRecordCell : HTBehaviour
{
    public ChooseRecord chooseRecord;

    public Text _title;
    public string __title;
    public Text _time;
    public string __time;

    public int recordId { get; set; }

    public string title
    {
        get => __title;
        set { if (_title != null) _title.text = value; __title = value; }
    }
    public string time
    {
        get => __time;
        set { if (_time != null) _time.text = value; __time = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            chooseRecord.Choose(this);
        });
    }

    public void SetRecordInfo(RecordInfo recordInfo)
    {
        this.recordId = recordInfo.id;
        this.time = recordInfo.timeString;
        this.title = recordInfo.title;
    }
}
