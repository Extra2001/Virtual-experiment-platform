using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RightButton : HTBehaviour
{
    private bool MouseOn = false;

    private Tuple<Vector3, Vector3> Max(MeshFilter[] filters)
    {
        double max = 0;
        Vector3 ret = new Vector3(), retCenter = new Vector3();
        foreach (var item in filters)
        {
            var tt = item.sharedMesh.bounds.size.x * item.sharedMesh.bounds.size.y * item.sharedMesh.bounds.size.z;
            if (tt > max)
            {
                max = tt;
                ret = item.sharedMesh.bounds.size;
                retCenter = item.sharedMesh.bounds.center;
            }
        }
        return new Tuple<Vector3, Vector3>(ret, retCenter);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                print("hit:" + hit.collider.gameObject.name);
            }
        }
    }
}
