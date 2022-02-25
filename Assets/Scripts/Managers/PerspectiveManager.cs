using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveManager : HTBehaviour
{
    public static PerspectiveManager Instance;
    public Camera front;
    public Camera side;
    public Camera top;
    public bool showed => front.enabled;

    private void Start()
    {
        Instance = this;
    }

    public void ShowPerspective()
    {
        front.enabled = side.enabled = top.enabled = true;
    }
    public void HidePerspective()
    {
        front.enabled = side.enabled = top.enabled = false;
    }
}
