using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Record
{
    public class RecordCell : HTBehaviour
    {
        public Text _title;
        public Text _time;
        public Button _LoadButton;
        public Button _DeleteButton;
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

        // Start is called before the first frame update
        void Start()
        {
            if (_LoadButton != null)
                _LoadButton.onClick.AddListener(LoadRecord);
            if (_DeleteButton != null)
                _DeleteButton.onClick.AddListener(DeleteRecord);
        }

        public void SetRecordInfo(RecordInfo recordInfo)
        {
            this.recordId = recordInfo.id;
            this.time = recordInfo.timeString;
            this.title = recordInfo.title;
        }

        void LoadRecord()
        {
            RecordManager.records[recordId].LoadToTempRecord();
        }

        void DeleteRecord()
        {
            RecordManager.records[recordId].Delete();
        }
    }
}