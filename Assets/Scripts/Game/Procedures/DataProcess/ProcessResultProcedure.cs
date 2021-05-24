using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
/// <summary>
/// 新建流程
/// </summary>
public class ProcessResultProcedure : ProcedureBase
{
    
    /// <summary>
    /// 流程初始化
    /// </summary>
    public override void OnInit()
    {
		base.OnInit();
    }

    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
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
        if (rec.complexQuantityMoedel.AverageExpression == null)
        {
            ShowModel($"合成物理量的主值还未计算");
            return;
        }
        if (rec.complexQuantityMoedel.UncertainExpression == null)
        {
            ShowModel($"合成物理量的不确定度还未计算");
            return;
        }
        Main.m_UI.OpenResidentUI<ProcessResult>();
        base.OnEnter(lastProcedure);
    }

    private void ShowModel(string message)
    {
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
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<ProcessResult>();
        base.OnLeave(nextProcedure);
    }

    /// <summary>
    /// 流程帧刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    /// <summary>
    /// 流程帧刷新（秒）
    /// </summary>
    public override void OnUpdateSecond()
    {
        base.OnUpdateSecond();
    }
}