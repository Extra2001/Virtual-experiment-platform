/************************************************************************************
    作者：荆煦添
    描述：添加物理量UI逻辑类
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 添加物理量UI逻辑类
/// </summary>
[UIResource(null, null, "UI/PreExperiment/AddQuantityPanel")]
public class AddValueUILogic : UILogicResident
{
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
        UIEntity.GetComponentInChildren<QuantityManage>(true).LoadQuantities();
        if (args.Length > 0)
        {
            if ((bool)args[0])
            {
                UIShowHideHelper.ShowFromButtom(UIEntity);
                Main.m_UI.PlaceTopUI(GetType());
                UIEntity.GetComponentInChildren<NextButtonOnAddQuantities>(true).gameObject.SetActive(false);
                UIEntity.GetComponentInChildren<BackButton>(true).inverse = true;
                UIEntity.GetComponentInChildren<BackButton>(true).UILogic = GetType();
            }
        }
        else
        {
            UIEntity.GetComponentInChildren<NextButtonOnAddQuantities>(true).gameObject.SetActive(true);
            UIEntity.GetComponentInChildren<BackButton>(true).inverse = false;
        }
    }
}