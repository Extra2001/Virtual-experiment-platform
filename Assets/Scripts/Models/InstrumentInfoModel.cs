using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class InstrumentInfoModel
{
    [Serializable]
    public class Vector3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }
    public Type instrumentType = null;

    public Vector3 position = new Vector3();

    public Vector3 rotation = new Vector3();
}