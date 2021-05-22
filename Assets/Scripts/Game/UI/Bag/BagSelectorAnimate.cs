using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class BagSelectorAnimate : MonoBehaviour
{
    public void Show()
    {
        CancelInvoke();
        UIShowHideHelper.ShowFromUp(gameObject, 50);
        gameObject.rectTransform().DOPunchRotation(new Vector3(0, 0, 20), 0.5f)
            .SetEase(Ease.OutExpo);
        Invoke(nameof(ResetRotation), .55f);
    }

    public void Hide()
    {
        UIShowHideHelper.HideToUp(gameObject);
    }

    private void ResetRotation()
    {
        gameObject.rectTransform().rotation = new Quaternion();
    }
}
