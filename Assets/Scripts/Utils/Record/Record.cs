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

    public Expression expressionKind { get; set; } = Expression.String;
    public string stringExpression { get; set; } = "";
    public string latexExpression { get; set; } = "";

    public InstrumentInfoModel showedInstrument { get; set; } = new InstrumentInfoModel();

    public List<ObjectsModel> objects { get; set; } = new List<ObjectsModel>()
    {
        new ObjectsModel()
        {
             Name = "立方体",
             DetailMessage = "纯正立方体",
             Integrated = true,
             PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/cubic.png"
        },
        new ObjectsModel()
        {
             Name = "圆柱",
             DetailMessage = "较高的一个圆柱",
             Integrated = true,
             PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/cylinder.png"
        },
        new ObjectsModel()
        {
             Name = "圆柱",
             DetailMessage = "较胖的一个圆柱",
             Integrated = true,
             PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/cylinder_low.png"
        },
    };
}
