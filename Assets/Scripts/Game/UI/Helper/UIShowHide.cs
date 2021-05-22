using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIShowHideHelper
{
    private static Vector2 GetScaledSize(Vector2 size)
    {
        return size * Screen.width / Main.m_UI.OverlayUIRoot.GetComponent<CanvasScaler>().referenceResolution.x;
    }

    private static Vector3 ScreenRightTop
    {
        get
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(Main.m_UI.OverlayUIRoot,
                new Vector2(Screen.width, Screen.height), null, out Vector3 point);
            return point;
        }
    }

    public static void ShowFromUp(GameObject UIEntity)
    {
        float fromUp = (ScreenRightTop.y - GetScaledSize(UIEntity.rectTransform().rect.size).y) / 2;
        ShowFromUp(UIEntity, fromUp);
    }

    public static void ShowFromUp(GameObject UIEntity, float fromUp, float x = 0)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // 计算初始高度
        var from = size.y / 2 + ScreenRightTop.y + 100;

        var position = rect.position;
        position.x = ScreenRightTop.x / 2 + x;
        position.y = from;

        UIEntity.rectTransform().position = position;
        // 计算结束高度
        var end = ScreenRightTop.y - fromUp - size.y / 2;

        UIEntity.rectTransform().DOMoveY(end, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void ShowFromButtom(GameObject UIEntity, float fromButtom, float x = 0)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // 计算初始高度
        var from = -1 * (size.y / 2) - 100;

        var position = rect.position;
        position.x = ScreenRightTop.x / 2 + x;
        position.y = from;

        UIEntity.rectTransform().position = position;
        // 计算结束高度
        var end = fromButtom + size.y / 2;

        UIEntity.rectTransform().DOMoveY(end, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideToButtom(GameObject UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // 计算结束高度
        var end = -1 * (size.y / 2) - 100;

        UIEntity.rectTransform().DOMoveY(end, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideToUp(GameObject UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);

        // 计算结束高度
        var end = size.y / 2 + ScreenRightTop.y + 100;

        UIEntity.rectTransform().DOMoveY(end, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
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
