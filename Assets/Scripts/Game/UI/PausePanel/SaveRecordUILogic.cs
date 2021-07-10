/************************************************************************************
    作者：荆煦添
    描述：保存存档面板UI逻辑类
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;
/// <summary>
/// 保存存档面板UI逻辑类
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
    }
}
