/************************************************************************************
    作者：张柯
    描述：物体数据模型
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//通用物体效应包，包含位置，角度，质量速度碰撞等等
//详细实现见子函数

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
    public float BaseScale { get => ObjectModel.baseScale; set => ObjectModel.baseScale = value; }
    public float Scale
    {
        get => ObjectModel.scale; set
        {
            gameObject.transform.localScale = new Vector3(value, value, value);
            ObjectModel.scale = value;
            ObjectModel.linescaleX = value;
            ObjectModel.linescaleY = value;
            ObjectModel.linescaleZ = value;
        }
    }
    public float LineScaleX
    {
        get => ObjectModel.linescaleX; set
        {
            gameObject.transform.localScale = new Vector3(ObjectModel.linescaleX, ObjectModel.linescaleY, ObjectModel.linescaleZ);
            ObjectModel.linescaleX = value;
        }
    }
    public float LineScaleY
    {
        get => ObjectModel.linescaleY; set
        {
            gameObject.transform.localScale = new Vector3(ObjectModel.linescaleX, ObjectModel.linescaleY, ObjectModel.linescaleZ);
            ObjectModel.linescaleY = value;
        }
    }
    public float LineScaleZ
    {
        get => ObjectModel.linescaleZ; set
        {
            gameObject.transform.localScale = new Vector3(ObjectModel.linescaleX, ObjectModel.linescaleY, ObjectModel.linescaleZ);
            ObjectModel.linescaleZ = value;
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
            x.GetComponent<Collider>().isTrigger = !value;
        });
    }

    public bool Collider
    {
        get => !gameObject.GetComponentInChildren<Collider>().isTrigger;
        set => gameObject.GetComponentsInChildren<Collider>().Foreach((x, y) =>
            x.isTrigger = !value);
    }

    private void Update()
    {
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;
    }
}
