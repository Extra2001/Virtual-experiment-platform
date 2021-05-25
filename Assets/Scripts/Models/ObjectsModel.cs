using HT.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class MyVector3
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public static implicit operator Vector3(MyVector3 vector3)
    {
        return new Vector3(vector3.x, vector3.y, vector3.z);
    }
}

[Serializable]
public class MyVector4
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float w { get; set; }

    public static implicit operator Quaternion(MyVector4 vector3)
    {
        return new Quaternion(vector3.x, vector3.y, vector3.z, vector3.w);
    }
}

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
    public MyVector4 rotation { get; set; } = new MyVector4();
    public MyVector3 baseSize { get; set; } = new MyVector3();
    public float scale { get; set; } = 1;
    public float mass { get; set; } = 0;
}
