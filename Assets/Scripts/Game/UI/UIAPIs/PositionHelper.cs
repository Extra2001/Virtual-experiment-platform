using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PositionHelper
{
    public static Vector2 GetScaledSize(Vector2 size)
    {
        return size * Screen.width / Main.m_UI.OverlayUIRoot.GetComponent<CanvasScaler>().referenceResolution.x;
    }

    public static Vector3 ScreenRightTop
    {
        get
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(Main.m_UI.OverlayUIRoot,
                new Vector2(Screen.width, Screen.height), null, out Vector3 point);
            return point;
        }
    }

    public static Vector3 GetFloatPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rect;
        var size = GetScaledSize(rect.size);

        var mousePosition = Input.mousePosition;
        Vector3 position = new Vector3(mousePosition.x + size.x / 2 + 3, mousePosition.y + size.y / 2 + 3);

        if (mousePosition.x > ScreenRightTop.x / 2)
            position.x -= 2 * (size.x / 2 + 3);
        if (mousePosition.y > ScreenRightTop.y / 2)
            position.y -= 2 * (size.y / 2 + 3);
        return position;
    }

    /// <summary>
    /// 从屏幕上方弹出
    /// </summary>
    /// <param name="UIEntity"></param>
    /// <returns>屏幕外，屏幕内</returns>
    public static (float, float) GetUpPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // 计算初始高度
        var from = size.y / 2 + ScreenRightTop.y + 100;
        // 计算结束高度
        var end = ScreenRightTop.y - size.y / 2;
        return (from, end);
    }

    /// <summary>
    /// 从屏幕右侧弹出
    /// </summary>
    /// <param name="UIEntity"></param>
    /// <returns>屏幕外，屏幕内</returns>
    public static (float, float) GetRightPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // 计算初始高度
        var from = size.x / 2 + ScreenRightTop.x + 100;
        // 计算结束高度
        var end = ScreenRightTop.x - size.x / 2;
        return (from, end);
    }

    /// <summary>
    /// 从屏幕左侧弹出
    /// </summary>
    /// <param name="UIEntity"></param>
    /// <returns>屏幕外，屏幕内</returns>
    public static (float, float) GetLeftPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // 计算初始高度
        var from = - size.x / 2 - 100;
        // 计算结束高度
        var end = size.x / 2;
        return (from, end);
    }

    /// <summary>
    /// 从屏幕下方弹出
    /// </summary>
    /// <param name="UIEntity"></param>
    /// <returns>屏幕外，屏幕内</returns>
    public static (float, float) GetBottomPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // 计算初始高度
        var from = -1 * (size.y / 2) - 100;
        // 计算结束高度
        var end = size.y / 2;
        return (from, end);
    }

    public static void SetFloat(this RectTransform UIEntity)
    {
        UIEntity.position = UIEntity.GetFloatPosition();
        UIEntity.gameObject.SetActive(true);
    }
}
