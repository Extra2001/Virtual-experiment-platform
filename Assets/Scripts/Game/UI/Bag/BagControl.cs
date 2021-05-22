using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Bag/Bag")]
public class BagControl : UILogicTemporary
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
        GameManager.Instance.FPSable = false;
        UIEntity.FindChildren("BagControl").GetComponent<BagSelectorAnimate>()?.Show();
        UIEntity.FindChildren("Select_instrument").GetComponent<InstrumentChooserAnimate>()?.Show();
    }
    
	/// <summary>
	/// 关闭UI
	/// </summary>
    public override void OnClose()
    {
        GameManager.Instance.FPSable = true;
        base.OnClose();
    }

    public void Hide()
    {
        UIEntity.FindChildren("BagControl").GetComponent<BagSelectorAnimate>()?.Hide();
        UIEntity.FindChildren("Select_instrument").GetComponent<InstrumentChooserAnimate>()?.Hide();
        MainThread.Instance.DelayAndRun(100, () =>
        {
            Close();
        });
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
