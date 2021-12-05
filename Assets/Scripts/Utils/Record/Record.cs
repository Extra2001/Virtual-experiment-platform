/************************************************************************************
    作者：荆煦添、张峻凡
    描述：描述存档的数据模型
*************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存档数据模型
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

    /// <summary>
    /// 存档信息
    /// </summary>
    public RecordInfo info { get; set; } = new RecordInfo();
    /// <summary>
    /// 记录实验是否完成
    /// </summary>
    public bool experimentFinish { get; set; } = false;
    /// <summary>
    /// 实验流程栈，保存用户已经进行过的流程
    /// </summary>
    public List<Type> procedureStack { get; set; } = new List<Type>();
    /// <summary>
    /// 用户添加的物理量信息
    /// </summary>
    public List<QuantityModel> quantities { get; set; } = new List<QuantityModel>();
    /// <summary>
    /// 最终合成量的信息
    /// </summary>
    public ComplexQuantityModel complexQuantityModel { get; set; } = new ComplexQuantityModel();
    /// <summary>
    /// 正在计算的物理量的索引
    /// </summary>
    public int currentQuantityIndex { get; set; } = 0;
    /// <summary>
    /// 合成量表达式
    /// </summary>
    public string stringExpression { get; set; } = "";
    /// <summary>
    /// 仪器起始生成的位置
    /// </summary>
    public float[] instrumentStartPosition { get; set; } = new float[3];
    /// <summary>
    /// 被测物体起始生成位置
    /// </summary>
    public float[] objectStartPosition { get; set; } = new float[3];
    /// <summary>
    /// 已被生成的仪器
    /// </summary>
    public InstrumentInfoModel showedInstrument { get; set; } = null;
    /// <summary>
    /// 已被生成的仪器
    /// </summary>
    public List<InstrumentInfoModel> historyInstrument { get; set; } = new List<InstrumentInfoModel>();
    /// <summary>
    /// 已被生成的被测物体
    /// </summary>
    public ObjectsModel showedObject { get; set; } = null;
    /// <summary>
    /// 记录用户得分
    /// </summary>
    public ResultScore score { get; set; } = new ResultScore();
    /// <summary>
    /// 人物视角的位置坐标
    /// </summary>
    public MyVector3 FPSPosition = new MyVector3()
    {
        x = -58.759f,
        y = 6.4450f,
        z = 1.732f
    };
    /// <summary>
    /// 任务视角的旋转角
    /// </summary>
    public MyVector4 FPSRotation = new MyVector4()
    {
        x = 0,
        y = -0.7f,
        z = 0,
        w = 0.7f
    };
}
