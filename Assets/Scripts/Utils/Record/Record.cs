using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 默认存档数据模型类
/// </summary>
[Serializable]
public class Record
{
    public Record() { }

    public Record(int id)
    {
        info.id = id;
        info.time = DateTime.Now;
        info.title = $"存档{id}";
    }

    public Record(int id, string name)
    {
        info.id = id;
        info.time = DateTime.Now;
        info.title = name;
    }

    public RecordInfo info { get; set; } = new RecordInfo();

    public List<Type> procedureStack { get; set; } = new List<Type>();

    public List<QuantityModel> quantities { get; set; } = new List<QuantityModel>();

    public ComplexQuantityMoedel complexQuantityModel { get; set; } = new ComplexQuantityMoedel();//用于存储最终合成量的信息

    public int currentQuantityIndex { get; set; } = 0;

    public string stringExpression { get; set; } = "";
    
    public float[] instrumentStartPosition { get; set; } = new float[3];
    public float[] objectStartPosition { get; set; } = new float[3];

    public InstrumentInfoModel showedInstrument { get; set; } = null;
    public ObjectsModel showedObject { get; set; } = null;

    public List<ObjectsModel> objects { get; set; } = new List<ObjectsModel>();

    public MyVector3 FPSPosition = new MyVector3()
    {
        x = -58.759f,
        y = 10.282f,
        z = 1.732f
    };

    public MyVector4 FPSRotation = new MyVector4()
    {
        x = 0,
        y = -0.7f,
        z = 0,
        w = 0.7f
    };
}
