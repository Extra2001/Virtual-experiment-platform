/************************************************************************************
    作者：张峻凡、荆煦添
    描述：被测物体的信息
*************************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectsModel
{
    public int id { get; set; }
    public string Name { get; set; } = "";
    public string ResourcePath { get; set; } = "";
    public string PreviewImage { get; set; } = "";
    public string DetailMessage { get; set; } = "";
    public bool Integrated { get; set; } = false;

    public MyVector3 position { get; set; } = new MyVector3();
    public List<MyVector3> childrenPostition = new List<MyVector3>();
    public MyVector4 rotation { get; set; } = new MyVector4();
    public List<MyVector4> childrenRotation = new List<MyVector4>();
    public MyVector3 baseSize { get; set; } = new MyVector3();
    public float baseScale { get; set; } = 1;
    public float scale { get; set; } = 1;
    public float mass { get; set; } = 0;

    public float linescaleX { get; set; } = 1;
    public float linescaleY { get; set; } = 1;
    public float linescaleZ { get; set; } = 1;
}

[Serializable]
public class MyVector3 : IEquatable<MyVector3>
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public bool Equals(MyVector3 other)
    {
        return x == other.x && y == other.y && z == other.z;
    }

    public static implicit operator Vector3(MyVector3 vector3)
    {
        return new Vector3(vector3.x, vector3.y, vector3.z);
    }
}

[Serializable]
public class MyVector4 : IEquatable<MyVector4>
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float w { get; set; }

    public bool Equals(MyVector4 other)
    {
        return x == other.x && y == other.y && z == other.z && w == other.w ;
    }

    public static implicit operator Quaternion(MyVector4 vector3)
    {
        return new Quaternion(vector3.x, vector3.y, vector3.z, vector3.w);
    }
}