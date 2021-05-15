using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource("AssetBundleName", "AssetPath", "UI/DataProcess/MeasuredDataProcess")]
public class MeasuredDataProcess : UILogicResident
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
        QuantityModel quantity = (QuantityModel)args[0];
        UIEntity.GetComponent<UncertaintyInput>().Show(quantity);
        UIEntity.GetComponent<DealMeasuredDataInput>().Show(quantity);
    }

	/// <summary>
    /// 置顶UI
    /// </summary>
    public override void OnPlaceTop()
    {
        base.OnPlaceTop();
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
