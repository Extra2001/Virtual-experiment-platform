using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public interface IMeasurable
{
    double GetMeasureResult();
}

public interface IResetable
{
    void InstReset();
}

public interface IShowValuable
{
    void ShowValue(double value);
}

public abstract class InstrumentBase : EntityLogicBase, IMeasurable, IResetable, IShowValuable
{
    /// <summary>
    /// 仪器名称
    /// </summary>
    public abstract string InstName { get; }

    /// <summary>
    /// 上限
    /// </summary>
    public abstract double URV { get; }

    /// <summary>
    /// 下限
    /// </summary>
    public abstract double LRV { get; }

    /// <summary>
    /// 仪器误差限
    /// </summary>
    public abstract double ErrorLimit { get; }

    /// <summary>
    /// 随机误差
    /// </summary>
    public virtual double RandomError { get => UnityEngine.Random.Range(-1 * (float)RandomErrorLimit, (float)RandomErrorLimit); }

    /// <summary>
    /// 随机误差限
    /// </summary>
    public abstract double RandomErrorLimit { get; set; }

    /// <summary>
    /// 主值
    /// </summary>
    public abstract double MainValue { get; set; }

    /// <summary>
    /// 单位名称
    /// </summary>
    public abstract string Unit { get; }

    /// <summary>
    /// 单位符号
    /// </summary>
    public abstract string UnitSymbol { get; }

    /// <summary>
    /// 游戏对象
    /// </summary>
    public abstract GameObject gameObject { get; set; }

    public abstract Sprite previewImage { get; }

    /// <summary>
    /// 测量获取数据
    /// </summary>
    /// <returns>测量数据</returns>
    public abstract double GetMeasureResult();

    /// <summary>
    /// 重置仪器状态
    /// </summary>
    public abstract void InstReset();

    public abstract void ShowValue(double value);

    public override void OnShow()
    {
        Entity.layer = 11;
        Entity.tag = "Tools_Be_Moved";
        AddRightButton();
        base.OnShow();
    }

    private void AddRightButton()
    {
        var right = Entity.AddComponent<RightButton>();
    }

    public override void OnHide()
    {
        GameObject.Destroy(Entity.GetComponent<RightButton>());
    }
}
