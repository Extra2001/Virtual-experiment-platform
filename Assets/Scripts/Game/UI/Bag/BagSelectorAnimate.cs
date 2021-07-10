/************************************************************************************
    作者：荆煦添
    描述：打开关闭仪器抽屉的动画
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
using UnityEngine;

public class BagSelectorAnimate : MonoBehaviour
{
    /// <summary>
    /// 显示动画
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
    /// 隐藏动画
    /// </summary>
    public void Hide()
    {
        UIShowHideHelper.HideToUp(gameObject);
    }
    /// <summary>
    /// 重置旋转角以至正常角度
    /// </summary>
    private void ResetRotation()
    {
        gameObject.rectTransform().rotation = new Quaternion();
    }
}
