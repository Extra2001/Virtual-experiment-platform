using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeasurable {
    double GetMeasureResult();
}

public interface IResetable {
    void Reset();
}

public abstract class InstrumentBase : IMeasurable, IResetable {
    /// <summary>
    /// 仪器名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 上限
    /// </summary>
    public double URV { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    public double LRV { get; set; }

    /// <summary>
    /// 仪器误差限
    /// </summary>
    public virtual double ErrorLimit { get; set; }

    /// <summary>
    /// 随机误差
    /// </summary>
    public virtual double RandomError { get => UnityEngine.Random.Range(-1 * (float)RandomErrorLimit, (float)RandomErrorLimit); }

    /// <summary>
    /// 随机误差限
    /// </summary>
    public double RandomErrorLimit { get; set; }

    /// <summary>
    /// 主值
    /// </summary>
    public virtual double MainValue { get; set; }

    /// <summary>
    /// 单位名称
    /// </summary>
    public string Unit { get; set; }

    /// <summary>
    /// 单位符号
    /// </summary>
    public string UnitSymbol { get; set; }

    /// <summary>
    /// 游戏对象
    /// </summary>
    public GameObject gameObject { get; set; }

    /// <summary>
    /// 测量获取数据
    /// </summary>
    /// <returns>测量数据</returns>
    public abstract double GetMeasureResult();

    /// <summary>
    /// 重置仪器状态
    /// </summary>
    public abstract void Reset();
}
