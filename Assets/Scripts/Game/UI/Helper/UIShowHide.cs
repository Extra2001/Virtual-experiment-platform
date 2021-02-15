using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIShowHideHelper
{
    public static void ShowUpToDown(GameObject UIEntity)
    {
        UIEntity.transform.localPosition = new Vector3(0, 400, 0);
        UIEntity.transform.DOLocalMove(new Vector3(0, 0, 0), 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideUpToDown(GameObject UIEntity)
    {
        UIEntity.transform.DOLocalMove(new Vector3(0, 400, 0), 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo);
    }
}
