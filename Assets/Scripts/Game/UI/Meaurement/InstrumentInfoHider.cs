using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentInfoHider : HTBehaviour
{
    private void OnEnable()
    {
        transform.parent.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
