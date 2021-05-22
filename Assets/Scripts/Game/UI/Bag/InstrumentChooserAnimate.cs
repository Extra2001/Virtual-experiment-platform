using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class InstrumentChooserAnimate : MonoBehaviour
{
    public void Show()
    {
        UIShowHideHelper.ShowFromButtom(gameObject, 0);
    }

    public void Hide()
    {
        UIShowHideHelper.HideToButtom(gameObject);
    }
}
