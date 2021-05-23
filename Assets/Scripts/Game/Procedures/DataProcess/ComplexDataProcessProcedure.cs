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
    List<DataChart> dataChart = new List<DataChart>();

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
        DealData();
        base.OnEnter(lastProcedure);
    }

    private void DealData()
    {
        double avg, ua, u;
        for (int i = 0; i < RecordManager.tempRecord.quantities.Count; i++)
        {
            dataChart.Add(new DataChart());
            (avg, ua, u) = StaticMethods.CalcUncertain(RecordManager.tempRecord.quantities[i].Data, Main.m_Entity.GetEntities(RecordManager.tempRecord.quantities[i].InstrumentType)[0].Cast<InstrumentBase>().ErrorLimit);
            dataChart[i].Name = RecordManager.tempRecord.quantities[i].Symbol;
            dataChart[i].Average = avg.ToString();
            dataChart[i].Uncertain = u.ToString();
            dataChart[i].Ua = ua.ToString();
        }
    }

    public List<string> GetQuantitiesName()
    {
        return RecordManager.tempRecord.quantities.Select(x => x.Symbol).ToList();
    }

    public string GetStatisticValue(string quantityName, ComplexStatisticValue valueKind)
    {
        string result = "error";
        for (int i = 0; i < dataChart.Count; i++)
        {
            if(dataChart[i].Name == quantityName)
            {
                if (ComplexStatisticValue.Average == valueKind)
                {
                    result = dataChart[i].Average;
                }else if(ComplexStatisticValue.Uncertain == valueKind)
                {
                    result = dataChart[i].Uncertain;
                }
                break;
            }
        }
        Debug.Log(result);
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