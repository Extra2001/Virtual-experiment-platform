using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIShowHideHelper
{
    public static void ShowFromUp(GameObject UIEntity)
    {
        UIEntity.transform.localPosition = new Vector3(0, 500, 0);
        UIEntity.transform.DOLocalMove(new Vector3(0, 0, 0), 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideToUp(GameObject UIEntity)
    {
        UIEntity.transform.DOLocalMove(new Vector3(0, 500, 0), 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo);
    }

    public static void ShowFromRight(GameObject UIEntity)
    {
        Log.Info(UIEntity.transform.localPosition.ToString());
        var tmp = UIEntity.transform.localPosition;
        tmp.x = 600;
        UIEntity.transform.localPosition = tmp;
        UIEntity.transform.DOLocalMoveX(280, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideToRight(GameObject UIEntity)
    {
        UIEntity.transform.DOLocalMoveX(600, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo);
    }
}
