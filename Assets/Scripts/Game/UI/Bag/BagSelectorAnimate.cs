using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class BagSelectorAnimate : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    TweenerCore<Vector3, Vector3, VectorOptions> ani;

    // Start is called before the first frame update
    public void Show()
    {
        var old = transform.position;
        float oldy = transform.position.y;
        old.y = 800;
        transform.position = old;
        transform.DOMoveY(oldy, 0.3f)
            .SetEase(Ease.OutExpo);
        transform.DOPunchRotation(new Vector3(0, 0, 20), 0.5f)
            .SetEase(Ease.OutExpo);
    }

    public Vector3 Hide()
    {
        var oldy = transform.position;
        ani = transform.DOMoveY(800, 0.3f)
            .SetEase(Ease.OutExpo);
        return oldy;
    }

    public void Kill()
    {
        ani.Kill();
    }
}
