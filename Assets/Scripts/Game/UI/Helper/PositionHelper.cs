using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PositionHelper
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

    public static void SetFloat(this RectTransform UIEntity)
    {
        var rect = UIEntity.rect;
        var size = GetScaledSize(rect.size);

        var mousePosition = Input.mousePosition;
        Vector3 position = new Vector3(mousePosition.x + size.x / 2 + 3, mousePosition.y + size.y / 2 + 3);

        if (mousePosition.x > ScreenRightTop.x / 2)
            position.x -= 2 * (size.x / 2 + 3);
        if (mousePosition.y > ScreenRightTop.y / 2)
            position.y -= 2 * (size.y / 2 + 3);

        UIEntity.position = position;
        UIEntity.gameObject.SetActive(true);
    }
}
