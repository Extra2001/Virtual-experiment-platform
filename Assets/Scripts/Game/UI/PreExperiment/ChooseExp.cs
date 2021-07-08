using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseExp : HTBehaviour
{
    public string expName;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Initializer.GetPresetRecord(expName).Load();
        });
    }
}
