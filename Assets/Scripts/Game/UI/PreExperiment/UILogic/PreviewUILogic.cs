/************************************************************************************
    作者：荆煦添
    描述：预览物理量UI逻辑类
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 预览物理量UI逻辑类
/// </summary>
[UIResource(null, null, "UI/PreExperiment/PreviewPanel")]
public class PreviewUILogic : UILogicResident
{
	/// <summary>
	/// 打开UI
	/// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        KeyboardManager.Instance.work = false;
        UIEntity.GetComponent<PreviewExp>()?.Show();
        if(args.Length > 0)
        {
            if ((bool)args[0])
            {
                UIShowHideHelper.ShowFromButtom(UIEntity);
                Main.m_UI.PlaceTopUI(GetType());
                UIEntity.GetComponentInChildren<NextButtonOnPreviewConfirm>(true).gameObject.SetActive(false);
                UIEntity.GetComponentInChildren<BackButton>(true).inverse = true;
                UIEntity.GetComponentInChildren<BackButton>(true).UILogic = GetType();
            }
        }
        else
        {
            UIEntity.GetComponentInChildren<NextButtonOnPreviewConfirm>(true).gameObject.SetActive(true);
            UIEntity.GetComponentInChildren<BackButton>(true).inverse = false;
        }
    }

    public override void OnClose()
    {
        base.OnClose();
        KeyboardManager.Instance.work = true;
    }
}
