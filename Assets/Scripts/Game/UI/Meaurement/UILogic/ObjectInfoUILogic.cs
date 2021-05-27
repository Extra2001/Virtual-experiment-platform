using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using System;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Measurment/ObjectInfoPanel")]
public class ObjectInfoUILogic : UILogicTemporary
{
    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        if (args.Length > 0)
        {
            var value = args[0] as ObjectValue;
            UIEntity.GetComponent<ObjectInfo>().Show(value);
        }
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public override void OnClose()
    {
        base.OnClose();
    }

    /// <summary>
    /// 销毁UI
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// UI逻辑刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}