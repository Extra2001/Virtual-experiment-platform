using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using System;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Measurment/DataTable/DataTable")]
public class DatatableUILogic : UILogicTemporary
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
        var tmp = UIEntity.transform.position;
        tmp.x = -800;
        UIEntity.transform.position = tmp;
        base.OnOpen(args);
    }

    public void Show()
    {
        UIEntity.GetComponent<DataTable>().Show();
        UIShowHideHelper.ShowFromLeft(UIEntity);
    }

    public void Show(Type type)
    {
        UIEntity.GetComponent<DataTable>().Show(type);
        UIShowHideHelper.ShowFromLeft(UIEntity);
    }

    public void Show<T>() where T : InstrumentBase
    {
        UIEntity.GetComponent<DataTable>().Show(typeof(T));
        UIShowHideHelper.ShowFromLeft(UIEntity);
    }

    public void Hide()
    {
        UIShowHideHelper.HideToLeft(UIEntity);
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
