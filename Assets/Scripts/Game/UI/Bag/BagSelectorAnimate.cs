/************************************************************************************
    ���ߣ�������
    �������򿪹ر���������Ķ���
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
using UnityEngine;

public class BagSelectorAnimate : MonoBehaviour
{
    /// <summary>
    /// ��ʾ����
    /// </summary>
    public void Show()
    {
        CancelInvoke();
        UIShowHideHelper.ShowFromUp(gameObject, 50);
        gameObject.rectTransform().DOPunchRotation(new Vector3(0, 0, 20), 0.5f)
            .SetEase(Ease.OutExpo);
        Invoke(nameof(ResetRotation), .55f);
    }
    /// <summary>
    /// ���ض���
    /// </summary>
    public void Hide()
    {
        UIShowHideHelper.HideToUp(gameObject);
    }
    /// <summary>
    /// ������ת�����������Ƕ�
    /// </summary>
    private void ResetRotation()
    {
        gameObject.rectTransform().rotation = new Quaternion();
    }
}
