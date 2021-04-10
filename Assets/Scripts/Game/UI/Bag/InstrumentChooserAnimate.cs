using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class InstrumentChooserAnimate : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    TweenerCore<Vector3, Vector3, VectorOptions> ani;

    public void Show()
    {
        var old = transform.position;
        float oldy = transform.position.y;
        old.y = -800;
        transform.position = old;
        transform.DOMoveY(oldy, 0.3f)
            .SetEase(Ease.OutExpo);
        //transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1), 0.3f)
        //    .SetEase(Ease.OutExpo);
    }

    public Vector3 Hide()
    {
        var oldy = transform.position;
        ani = transform.DOMoveY(-800, 0.3f)
            .SetEase(Ease.OutExpo);
        return oldy;
    }

    public void Kill()
    {
        ani.Kill();
    }
}
