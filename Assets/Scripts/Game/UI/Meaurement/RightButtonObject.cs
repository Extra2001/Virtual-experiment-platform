using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEditor;
using System.Linq;

public class RightButtonObject : HTBehaviour
{
    public ObjectValue objectValue;
    public int index;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (var item in gameObject.transform.GetComponentsInChildren<Collider>(true))
                    if (hit.collider.gameObject.GetInstanceID().Equals(item.gameObject.GetInstanceID()))
                        Main.m_UI.OpenTemporaryUI<ObjectInfoUILogic>(objectValue);
            }
        }

        objectValue.childrenPostition[index] = transform.localPosition.GetMyVector();
        objectValue.childrenRotation[index] = transform.localRotation.GetMyVector();
    }
}
