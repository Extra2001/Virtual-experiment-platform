/************************************************************************************
    作者：荆煦添
    描述：提示器1的UI逻辑类
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

/// <summary>
/// 提示器1的UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Indicator/Indicator")]
public class Indicator1UILogic : IndicatorBase
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
                KeyText = item;
            else if (item.gameObject.name.Equals("MessageText"))
                MessageText = item;
        }
        base.OnInit();
    }
    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="key"></param>
    /// <param name="message"></param>
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
        UIShowHideHelper.ShowFromUpRight(UIEntity, 15, 15);
        base.OnOpen(args);
    }
}
