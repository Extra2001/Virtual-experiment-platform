using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Newtonsoft.Json;

public class TestSave : HTBehaviour
{
     // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            string data = JsonConvert.SerializeObject(RecordManager.tempRecord);
            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "hhh.txt"), data);
            Debug.Log("已写入");
        });
    }
}
