using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightButton : HTBehaviour
{
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Log.Info(gameObject.name);
        }
    }
}
