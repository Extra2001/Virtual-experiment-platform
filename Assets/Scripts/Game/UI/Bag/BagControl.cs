/************************************************************************************
    作者：张峻凡
    描述：仪器和被测物体UI逻辑类
*************************************************************************************/
using HT.Framework;

/// <summary>
/// 仪器和被测物体UI逻辑类
/// </summary>
[UIResource(null, null, "UI/Bag/Bag")]
public class BagControl : UILogicTemporary
{
	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        GameManager.Instance.FPSable = false;
        UIEntity.GetComponent<BagSelector>()?.Show();
    }
    
	/// <summary>
	/// 关闭UI
	/// </summary>
    public override void OnClose()
    {
        GameManager.Instance.FPSable = true;
        base.OnClose();
    }

    /// <summary>
    /// 隐藏并关闭UI(显示动画)
    /// </summary>
    public void Hide()
    {
        UIEntity.GetComponent<BagSelector>()?.Hide();
        MainThread.Instance.DelayAndRun(300, () =>
        {
            Close();
        });
    }
}
