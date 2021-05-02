using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineWidthSync : HTBehaviour
{
    public GameObject Line;
    public GameObject Panel;

    // Update is called once per frame
    void Update()
    {
        //var rect = Line.rectTransform().rect;
        //Line.rectTransform().rect.Set(rect.x, rect.y, Panel.rectTransform().rect.width, rect.height);
    }
}
