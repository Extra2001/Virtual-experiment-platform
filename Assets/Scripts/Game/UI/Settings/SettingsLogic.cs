/************************************************************************************
    作者：荆煦添
    描述：设置面板UI逻辑类
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;
/// <summary>
/// 设置面板UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Settings/Settings")]
public class SettingsLogicTemporary : UILogicTemporary
{
	/// <summary>
	/// 初始化
	/// </summary>
    public override void OnInit()
    {
        base.OnInit();
        UIEntity.FindChildren("TweenObject").FindChildren("BackButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            NavigateBack();
        });
    }

	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        UIShowHideHelper.ShowFromUp(UIEntity);
        base.OnOpen(args);
    }
}
