using HT.Framework;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using System;

namespace UI.NewRecordSelector
{
    class Example03 : HTBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var newRecord = new ItemData()
            {
                RecordName = "新建存档",
                Time = DateTime.Now.ToString("f")
            };
            var items = Enumerable.Range(0, 20)
                .Select(i => new ItemData()
                {
                    RecordName = $"存档 {string.Format("{0:d2}", i)}",
                    Time = DateTime.Now.ToString("f")
                }).ToArray();

            scrollView.UpdateData(new ItemData[] { newRecord }.Concat(items).ToArray());
            scrollView.SelectCell(0);
        }
    }
}
