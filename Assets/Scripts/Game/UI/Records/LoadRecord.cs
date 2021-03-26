using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Record
{
    public class LoadRecord : HTBehaviour
    {
        public GameObject RecordObj;
        public GameObject EmptyObj;
        private GameObject emptyObj;
        private List<RecordCell> showedRecords = new List<RecordCell>();

        void Start()
        {
            Main.m_Event.Subscribe<RecordUpdateEventHandler>(RefreshRecord);
            RefreshRecord();
        }

        private void RefreshRecord()
        {
            foreach (var item in showedRecords)
                Destroy(item.gameObject);
            showedRecords.Clear();
            if (emptyObj != null)
                Destroy(emptyObj);
            emptyObj = null;
            var list = RecordManager.recordInfos;
            if (list.Count == 0)
                emptyObj = Instantiate<GameObject>(EmptyObj, gameObject.transform);
            else
                foreach (var item in list)
                {
                    var RecordCell = Instantiate<GameObject>(RecordObj, gameObject.transform).GetComponent<RecordCell>();
                    RecordCell.SetRecordInfo(item);
                    showedRecords.Add(RecordCell);
                }
        }

        void OnDestroy()
        {
            Main.m_Event.Unsubscribe<RecordUpdateEventHandler>(RefreshRecord);
        }
    }

}