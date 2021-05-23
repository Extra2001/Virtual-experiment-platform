using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using System;
using System.Linq;
/// <summary>
/// 新建流程
/// </summary>
public class ComplexDataProcessProcedure : ProcedureBase
{
    private List<QuantityModel> quantities => RecordManager.tempRecord.quantities;
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
        Main.m_UI.OpenResidentUI<ComplexData>();        
        base.OnEnter(lastProcedure);
    }    

    public List<string> GetQuantitiesName()
    {
        return RecordManager.tempRecord.quantities.Select(x => x.Symbol).ToList();
    }

    public string GetStatisticValue(string quantityName, ComplexStatisticValue valueKind)
    {
        string result = "error";
        var item = quantities.Where(x => x.Symbol.Equals(quantityName)).FirstOrDefault();
            if(item.Name == quantityName)
            {
                if (ComplexStatisticValue.Average == valueKind)
                {
                result = item.Average.ToString();
                }else if(ComplexStatisticValue.Uncertain == valueKind)
                {
                    result = item.Uncertain.ToString();
                }
            }
        return result;
        //throw new NotImplementedException();
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.m_UI.CloseUI<ComplexData>();
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

public enum ComplexStatisticValue
{
    Average,
    Uncertain
}