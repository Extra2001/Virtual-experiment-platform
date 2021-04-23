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

    public int currentQuantityIndex { get; set; } = 0;

    public Expression expressionKind { get; set; } = Expression.String;
    public string stringExpression { get; set; } = "";
    public string latexExpression { get; set; } = "";
    
    public float[] InstrumentStartPosition { get; set; } = new float[3];
    public float[] ObjectStartPosition { get; set; } = new float[3];

    public Type Instrumenttype { get; set; } = null;
    public ObjectsModel ShowedObject { get; set; } = null;
    public float ObjectRate { get; set; } = 1f; //物体放大或缩小倍率


    public InstrumentInfoModel showedInstrument { get; set; } = new InstrumentInfoModel();

    public List<ObjectsModel> objects { get; set; } = new List<ObjectsModel>();
}
