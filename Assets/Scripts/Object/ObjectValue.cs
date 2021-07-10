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

    public List<MyVector3> childrenPostition => ObjectModel.childrenPostition;

    public List<MyVector4> childrenRotation => ObjectModel.childrenRotation;

    public float Scale
    {
        get => ObjectModel.scale; set
        {
            gameObject.transform.localScale = new Vector3(value, value, value);
            ObjectModel.scale = value;
        }
    }
    public float Mass { get => ObjectModel.mass; set => ObjectModel.mass = value; }
    public bool Gravity
    {
        get => gameObject.GetComponentInChildren<Rigidbody>().useGravity;
        set => gameObject.GetComponentsInChildren<Rigidbody>().Foreach((x, y) =>
        {
            x.useGravity = value;
            x.isKinematic = !value;
            x.GetComponent<MeshCollider>().isTrigger = !value;
        });
    }

    public bool Collider
    {
        get => !gameObject.GetComponentInChildren<MeshCollider>().isTrigger;
        set => gameObject.GetComponentsInChildren<MeshCollider>().Foreach((x, y) =>
            x.isTrigger = !value);
    }

    private void Update()
    {
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;
    }
}