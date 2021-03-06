using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Record/SaveRecord")]
public class SaveRecordUILogic : UILogicTemporary
{
    public Button ConfirmButton { get; set; }
    public Button CancelButton { get; set; }

	/// <summary>
	/// 初始化
	/// </summary>
    public override void OnInit()
    {
        base.OnInit();
        AddInnerLogic();

        Log.Info("保存存档面板 已加载");
    }

    private void AddInnerLogic()
    {
        foreach (var item in UIEntity.GetComponentsInChildren<Button>(true))
        {
            if (item.gameObject.name.Equals("ConfirmButton"))
            {
                ConfirmButton = item;
            }
            else if (item.gameObject.name.Equals("CancelButton"))
            {
                CancelButton = item;
            }
        }
        CancelButton.onClick.AddListener(() =>
        {
            NavigateBack();
        });
    }

	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        UIShowHideHelper.ShowFromUp(UIEntity);
        Log.Info("保存存档面板 已打开");
    }
    
	/// <summary>
	/// 关闭UI
	/// </summary>
    public override void OnClose()
    {
        base.OnClose();
        Log.Info("保存存档面板 已关闭");
    }

	/// <summary>
	/// 销毁UI
	/// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        Log.Info("保存存档面板 已销毁");
    }

	/// <summary>
	/// UI逻辑刷新
	/// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
