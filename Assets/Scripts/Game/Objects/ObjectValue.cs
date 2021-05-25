using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectValue : HTBehaviour
{
    public ObjectsModel ObjectModel;

    public Vector3 BaseSize { get => ObjectModel.baseSize; set => ObjectModel.baseSize = value.GetMyVector(); }
    public Vector3 Size { get => BaseSize * Scale; }
    public Vector3 Position
    {
        get => ObjectModel.position; set
        {
            gameObject.transform.position = value;
            ObjectModel.position = value.GetMyVector();
        }
    }
    public Quaternion Rotation
    {
        get => ObjectModel.rotation; set
        {
            gameObject.transform.rotation = value;
            ObjectModel.rotation = value.GetMyVector();
        }
    }
    public float Scale
    {
        get => ObjectModel.scale; set
        {
            gameObject.transform.localScale = new Vector3(value, value, value);
            ObjectModel.scale = value;
        }
    }
    public float Mass { get => ObjectModel.mass; set => ObjectModel.mass = value; }

    private void OnDestroy()
    {
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;
    }
}
