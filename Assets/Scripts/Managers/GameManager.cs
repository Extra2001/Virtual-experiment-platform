using HT.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviorManager<GameManager>
{
    List<Type> ProcedureStack = new List<Type>();

    public void SwitchBackToStart()
    {
        Main.m_Procedure.SwitchProcedure(ProcedureStack[0]);
        ProcedureStack.RemoveRange(1, ProcedureStack.Count - 1);
    }

    public void SwitchBackProcedure()
    {
        string hh = "";
        ProcedureStack.ForEach(x => hh+=x.Name);
        Debug.Log(hh);
        if (ProcedureStack.Count <= 1) return;
        Main.m_Procedure.SwitchProcedure(ProcedureStack[ProcedureStack.Count - 2]);
        ProcedureStack.RemoveAt(ProcedureStack.Count - 1);
    }

    private void Start()
    {
        ProcedureStack.Add(Main.m_Procedure.CurrentProcedure.GetType());
        Main.m_Event.Subscribe<Sitdown>(WhenSitdown);
        Main.m_Event.Subscribe<StartNewExpEventHandler>(StartNewExp);
        Main.m_Event.Subscribe<ChooseExpEventHandler>(ChooseExp);
        Main.m_Event.Subscribe<AddValueEventHandler>(AddValue);
        Main.m_Event.Subscribe<EnterExpressionEventHandler>(EnterExpression);
        Main.m_Event.Subscribe<PreviewConfirmEventHandler>(PreviewConfirm);
    }

    private void WhenSitdown(object sender, EventHandlerBase handler)
    {
        Main.m_Procedure.SwitchProcedure<OnChair>();
        ProcedureStack.Add(typeof(OnChair));
    }

    private void StartNewExp()
    {
        Main.m_Procedure.SwitchProcedure<ChooseExpProcedure>();
        ProcedureStack.Add(typeof(ChooseExpProcedure));
        string hh = "";
        ProcedureStack.ForEach(x => hh += x.Name);
        Debug.Log(hh);
    }

    private void ChooseExp(EventHandlerBase handler)
    {
        if ((handler as ChooseExpEventHandler).expId == 0)
        {
            Main.m_Procedure.SwitchProcedure<AddValueProcedure>();
            ProcedureStack.Add(typeof(AddValueProcedure));
        }
        else
        {
            // 加载预制实验
        }
        string hh = "";
        ProcedureStack.ForEach(x => hh += x.Name);
        Debug.Log(hh);
    }

    private void AddValue()
    {
        Main.m_Procedure.SwitchProcedure<EnterExpressionProcedure>();
        ProcedureStack.Add(typeof(EnterExpressionProcedure));
        string hh = "";
        ProcedureStack.ForEach(x => hh += x.Name);
        Debug.Log(hh);
    }

    private void EnterExpression()
    {
        Main.m_Procedure.SwitchProcedure<PreviewProcedure>();
        ProcedureStack.Add(typeof(PreviewProcedure));
        string hh = "";
        ProcedureStack.ForEach(x => hh += x.Name);
        Debug.Log(hh);
    }

    private void PreviewConfirm()
    {
        Main.m_Procedure.SwitchProcedure<EnterClassroomProcedure>();
        ProcedureStack.Add(typeof(EnterClassroomProcedure));
        string hh = "";
        ProcedureStack.ForEach(x => hh += x.Name);
        Debug.Log(hh);
    }
}
