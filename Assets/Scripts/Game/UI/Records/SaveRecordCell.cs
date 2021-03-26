using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils.Record;

public class SaveRecordCell : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

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
