/************************************************************************************
    ���ߣ�������
    ������UIλ�ü�����
*************************************************************************************/
using DG.Tweening;
using HT.Framework;
using System;
using System.Collections;
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

    public static (Vector3, Vector2) GetFloatPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rect;
        var size = GetScaledSize(rect.size);

        var mousePosition = Input.mousePosition;
        Vector3 position;
        if (UIEntity.pivot.x > 0.4 && UIEntity.pivot.x < 0.6 && UIEntity.pivot.y > 0.4 && UIEntity.pivot.y < 0.6)
        {
            position = new Vector3(mousePosition.x - size.x / 2 - 3, mousePosition.y + size.y / 2 + 3);

            if (mousePosition.y > ScreenRightTop.y / 2)
                position.y -= 2 * (size.y / 2 + 3);

            if (position.x - size.x / 2 < 0)
                position.x += size.x / 2 - position.x;

            if (position.y + size.y / 2 > ScreenRightTop.y)
                position.y -= position.y + size.y / 2 - ScreenRightTop.y;
            if (position.y - size.y / 2 < 0)
                position.y += size.y / 2 - position.y;
            return (position, size);
        }
        else if(UIEntity.pivot.x > 0.9 && UIEntity.pivot.y > 0.9)
        {
            position = new Vector3(mousePosition.x - 3, mousePosition.y - 3);

            if (position.x - size.x < 0)
                position.x += size.x - position.x;
            if (position.y - size.y < 0)
                position.y += size.y - position.y;
            return (position, size);
        }
        else if (UIEntity.pivot.x > 0.9 && UIEntity.pivot.y < 0.1)
        {
            position = new Vector3(mousePosition.x - 3, mousePosition.y + 3);

            if (position.x - size.x < 0)
                position.x += size.x - position.x;
            if (position.y + size.y > ScreenRightTop.y)
                position.y -= position.y + size.y - ScreenRightTop.y;
            return (position, size);
        }
        else if (UIEntity.pivot.x < 0.1 && UIEntity.pivot.y > 0.9)
        {
            position = new Vector3(mousePosition.x + 3, mousePosition.y - 3);

            if (position.x + size.x >ScreenRightTop.x)
                position.x -= position.x + size.x - ScreenRightTop.x;
            if (position.y - size.y < 0)
                position.y += size.y - position.y;
            return (position, size);
        }
        else
        {
            position = new Vector3(mousePosition.x + 3, mousePosition.y + 3);

            if (position.x + size.x > ScreenRightTop.x)
                position.x -= position.x + size.x - ScreenRightTop.x;
            if (position.y + size.y > ScreenRightTop.y)
                position.y -= position.y + size.y - ScreenRightTop.y;
            return (position, size);
        }
    }

    /// <summary>
    /// ����Ļ�Ϸ�����
    /// </summary>
    /// <param name="UIEntity"></param>
    /// <returns>��Ļ�⣬��Ļ��</returns>
    public static (float, float) GetUpPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // �����ʼ�߶�
        var from = size.y / 2 + ScreenRightTop.y + 100;
        // ��������߶�
        var end = ScreenRightTop.y - size.y / 2;
        return (from, end);
    }

    /// <summary>
    /// ����Ļ�Ҳ൯��
    /// </summary>
    /// <param name="UIEntity"></param>
    /// <returns>��Ļ�⣬��Ļ��</returns>
    public static (float, float) GetRightPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // �����ʼ�߶�
        var from = size.x / 2 + ScreenRightTop.x + 100;
        // ��������߶�
        var end = ScreenRightTop.x - size.x / 2;
        return (from, end);
    }

    /// <summary>
    /// ����Ļ��൯��
    /// </summary>
    /// <param name="UIEntity"></param>
    /// <returns>��Ļ�⣬��Ļ��</returns>
    public static (float, float) GetLeftPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // �����ʼ�߶�
        var from = -size.x / 2 - 100;
        // ��������߶�
        var end = size.x / 2;
        return (from, end);
    }

    /// <summary>
    /// ����Ļ�·�����
    /// </summary>
    /// <param name="UIEntity"></param>
    /// <returns>��Ļ�⣬��Ļ��</returns>
    public static (float, float) GetBottomPosition(this RectTransform UIEntity)
    {
        var rect = UIEntity.rectTransform().rect;
        var size = GetScaledSize(rect.size);
        // �����ʼ�߶�
        var from = -1 * (size.y / 2) - 100;
        // ��������߶�
        var end = size.y / 2;
        return (from, end);
    }

    public static bool SetFloat(this RectTransform UIEntity)
    {
        UIEntity.gameObject.SetActive(true);
        var (position, size) = UIEntity.GetFloatPosition();
        UIEntity.position = position;
        if (size.x == 0 || size.y == 0)
            return false;
        return true;
    }

    public static void SetFloatWithAnimation(this GameObject UIEntity, MonoBehaviour behaviour)
    {
        var mousePosition = Input.mousePosition;
        float x = mousePosition.x > ScreenRightTop.x / 2 ? 1 : 0;
        float y = mousePosition.y > ScreenRightTop.y / 2 ? 1 : 0;
        UIEntity.rectTransform().pivot = new Vector2(x, y);
        behaviour.StartCoroutine(CommonTools.DelayGet(UIEntity.rectTransform().SetFloat));
        UIEntity.transform.localScale = new Vector3(0, 0);
        UIEntity.transform.DOScale(1, 0.3f)
            .SetEase(Ease.OutExpo);
    }
}
