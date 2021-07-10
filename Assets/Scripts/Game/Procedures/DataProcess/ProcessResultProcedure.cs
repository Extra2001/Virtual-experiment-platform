/************************************************************************************
    作者：张峻凡
    描述：系统对用户指出错误并给出评价流程
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 系统对用户指出错误并给出评价流程
/// </summary>
public class ProcessResultProcedure : ProcedureBase
{
    /// <summary>
    /// 进入流程
    /// </summary>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        //如果有数据未输入则不能进入

        var rec = RecordManager.tempRecord;
        foreach(var item in rec.quantities)
        {
            if(item.UaExpression==null)
            {
                ShowModel($"物理量\"{item.Name}\"({item.Symbol})的A类不确定度还未计算");
                return; 
            }
            if (item.UbExpression == null)
            {
                ShowModel($"物理量\"{item.Name}\"({item.Symbol})的B类不确定度还未计算");
                return;
            }
            if (item.ComplexExpression == null)
            {
                ShowModel($"物理量\"{item.Name}\"({item.Symbol})的合成不确定度还未计算");
                return;
            }
        }
        if (rec.complexQuantityModel.AverageExpression == null)
        {
            ShowModel($"合成物理量的主值还未计算");
            return;
        }
        if (rec.complexQuantityModel.UncertainExpression == null)
        {
            ShowModel($"合成物理量的不确定度还未计算");
            return;
        }
        Main.m_UI.OpenResidentUI<ProcessResult>();
        base.OnEnter(lastProcedure);
    }

    private void ShowModel(string message)
    {
        //弹出报错信息UI
        MainThread.Instance.DelayAndRun(300, () =>
        {
            GameManager.Instance.SwitchBackProcedure();
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                ShowCancel = false,
                Title = new BindableString("错误"),
                Message = new BindableString(message)
            });
        });
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<ProcessResult>();
        base.OnLeave(nextProcedure);
    }
}