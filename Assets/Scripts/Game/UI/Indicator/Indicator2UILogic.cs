using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Indicator2")]
public class Indicator2UILogic : IndicatorBase
{
    private Text MessageText;
    private Text KeyText;
	/// <summary>
	/// 初始化
	/// </summary>
    public override void OnInit()
    {
        foreach (var item in UIEntity.GetComponentsInChildren<Text>(true))
        {
            if (item.gameObject.name.Equals("KeyText"))
            {
                KeyText = item;
            }
            else if (item.gameObject.name.Equals("MessageText"))
            {
                MessageText = item;
            }
        }
        base.OnInit();
    }

    public override void ShowIndicator(string key, string message)
    {
        MessageText.text = message;
        KeyText.text = key;
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        UIShowHideHelper.ShowFromRight(UIEntity);

        base.OnOpen(args);
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
