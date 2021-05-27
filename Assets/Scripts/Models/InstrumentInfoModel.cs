using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class InstrumentInfoModel
{
    public bool Valid = false;

    public Type instrumentType = null;

    public MyVector3 position = new MyVector3();
    public MyVector4 rotation = new MyVector4();

    public double MainValue = double.MinValue;
    public double RandomErrorLimit = double.MinValue;
}