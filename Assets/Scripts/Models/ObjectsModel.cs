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
    public string Name { get; set; } = "";
    public string ResourcePath { get; set; } = "";
    public string PreviewImage { get; set; } = "";
    public string DetailMessage { get; set; } = "";
    public bool Integrated { get; set; } = false;

    public Vector3 position { get; set; } = new Vector3();
    public Vector3 rotation { get; set; } = new Vector3();
    public Vector3 maxSize { get; set; } = new Vector3();
}
