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

    public static void ShowFromUp(GameObject UIEntity, float height)
    {
        UIEntity.transform.localPosition = new Vector3(0, 700, 0);
        UIEntity.transform.DOLocalMove(new Vector3(0, height, 0), 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void ShowFromButtom(GameObject UIEntity, float height)
    {
        UIEntity.transform.localPosition = new Vector3(0, -700, 0);
        UIEntity.transform.DOLocalMove(new Vector3(0, height, 0), 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideToButtom(GameObject UIEntity)
    {
        UIEntity.transform.DOLocalMove(new Vector3(0, -700, 0), 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo);
    }

    public static void HideToUp(GameObject UIEntity)
    {
        UIEntity.transform.DOLocalMove(new Vector3(0, 500, 0), 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo);
    }

    public static void ShowFromRight(GameObject UIEntity)
    {
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

    public static void ShowFromLeft(GameObject UIEntity)
    {
        var tmp = UIEntity.transform.localPosition;
        tmp.x = -900;
        UIEntity.transform.localPosition = tmp;
        UIEntity.transform.DOLocalMoveX(0, 0.5f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo);
    }

    public static void HideToLeft(GameObject UIEntity)
    {
        UIEntity.transform.DOLocalMoveX(-900, 0.5f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo);
    }
}
