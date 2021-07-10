/************************************************************************************
    ×÷Õß£º¾£ìãÌí
    ÃèÊö£ºUIÏÔÊ¾Òþ²Ø¹¤¾ß
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
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

    public static void ShowFromUp(GameObject UIEntity, float fromUp = 0, float x = 0)
    {
        var (from, end) = UIEntity.rectTransform().GetUpPosition();

        var position = UIEntity.rectTransform().position;
        position.y = from;
        position.x = (ScreenRightTop.x / 2 + x);
        UIEntity.rectTransform().position = position;

        UIEntity.rectTransform().DOMoveY(end - fromUp, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void ShowFromButtom(GameObject UIEntity, float fromButtom = 0, float x = 0)
    {
        var (from, end) = UIEntity.rectTransform().GetBottomPosition();

        var position = UIEntity.rectTransform().position;
        position.y = from;
        position.x = (ScreenRightTop.x / 2 + x);
        UIEntity.rectTransform().position = position;

        UIEntity.rectTransform().DOMoveY(end + fromButtom, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideToButtom(GameObject UIEntity)
    {
        var (from, _) = UIEntity.rectTransform().GetBottomPosition();

        UIEntity.rectTransform().DOMoveY(from, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideToUp(GameObject UIEntity)
    {
        var (from, _) = UIEntity.rectTransform().GetUpPosition();

        UIEntity.rectTransform().DOMoveY(from, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void ShowFromRight(GameObject UIEntity, float fromRight = 0, float y = 0)
    {
        var (from, end) = UIEntity.rectTransform().GetRightPosition();

        var position = UIEntity.rectTransform().position;
        position.x = from;
        position.y = (ScreenRightTop.y / 2 + y);
        UIEntity.rectTransform().position = position;

        UIEntity.rectTransform().DOMoveX(end - fromRight, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideToRight(GameObject UIEntity)
    {
        var (from, _) = UIEntity.rectTransform().GetRightPosition();

        UIEntity.rectTransform().DOMoveX(from, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void ShowFromLeft(GameObject UIEntity, float fromLeft = 0, float y = 0)
    {
        var (from, end) = UIEntity.rectTransform().GetLeftPosition();

        var position = UIEntity.rectTransform().position;
        position.x = from;
        position.y = (ScreenRightTop.y / 2 + y);
        UIEntity.rectTransform().position = position;

        UIEntity.rectTransform().DOMoveX(end + fromLeft, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void HideToLeft(GameObject UIEntity)
    {
        var (from, _) = UIEntity.rectTransform().GetLeftPosition();

        UIEntity.rectTransform().DOMoveX(from, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void ShowFromUpLeft(GameObject UIEntity, float fromUp = 0, float fromLeft = 0)
    {
        var (from, end) = UIEntity.rectTransform().GetLeftPosition();
        var (_, end1) = UIEntity.rectTransform().GetUpPosition();

        var position = UIEntity.rectTransform().position;
        position.x = from;
        position.y = end1 - fromUp;
        UIEntity.rectTransform().position = position;

        UIEntity.rectTransform().DOMoveX(end + fromLeft, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void ShowFromUpRight(GameObject UIEntity, float fromUp = 0, float fromRight = 0)
    {
        var (from, end) = UIEntity.rectTransform().GetRightPosition();
        var (_, end1) = UIEntity.rectTransform().GetUpPosition();

        var position = UIEntity.rectTransform().position;
        position.x = from;
        position.y = end1 - fromUp;
        UIEntity.rectTransform().position = position;

        UIEntity.rectTransform().DOMoveX(end - fromRight, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void ShowFromBottomRight(GameObject UIEntity, float fromBottom = 0, float fromRight = 0)
    {
        var (from, end) = UIEntity.rectTransform().GetRightPosition();
        var (_, end1) = UIEntity.rectTransform().GetBottomPosition();

        var position = UIEntity.rectTransform().position;
        position.x = from;
        position.y = end1 + fromBottom;
        UIEntity.rectTransform().position = position;

        UIEntity.rectTransform().DOMoveX(end - fromRight, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public static void ShowFromBottomLeft(GameObject UIEntity, float fromBottom = 0, float fromLeft = 0)
    {
        var (from, end) = UIEntity.rectTransform().GetLeftPosition();
        var (_, end1) = UIEntity.rectTransform().GetBottomPosition();

        var position = UIEntity.rectTransform().position;
        position.x = from;
        position.y = end1 + fromBottom;
        UIEntity.rectTransform().position = position;

        UIEntity.rectTransform().DOMoveX(end + fromLeft, 0.3f)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }
}
