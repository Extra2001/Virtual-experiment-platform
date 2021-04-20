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
public class ObjectsModel
{
    public class Vector3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }

    public int id { get; set; }
    public string Name { get; set; } = "";
    public string ResourcePath { get; set; } = "";
    public string PreviewImage { get; set; } = "";
    public string DetailMessage { get; set; } = "";
    public bool Integrated { get; set; } = false;
    public float Rate { get; set; } = 1f; //物体放大或缩小倍率

    public Vector3 position { get; set; } = new Vector3();
    public Vector3 rotation { get; set; } = new Vector3();
    public Vector3 maxSize { get; set; } = new Vector3();
}
