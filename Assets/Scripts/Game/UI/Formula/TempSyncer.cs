using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempSyncer : HTBehaviour
{
    public Text dest;
    public Text src;

    // Update is called once per frame
    void Update()
    {
        dest.text = src.text;
    }
}
