using HT.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Record;

public class GameManager : SingletonBehaviorManager<GameManager>
{
    List<Type> ProcedureStack { get => RecordManager.tempRecord.procedureStack; }

    public bool CanContinue { get => ProcedureStack.Count > 1; }

    public void SwitchBackToStart()
    {
        Main.m_Procedure.SwitchProcedure(ProcedureStack[0]);
    }

    public void SwitchBackProcedure()
    {
        if (ProcedureStack.Count <= 1) return;
        if (ProcedureStack.Count == 2)
        {
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                Message = new BindableString("继续返回将丢失当前进度，继续？"),
                ConfirmAction = () =>
                {
                    ProcedureStack.RemoveAt(ProcedureStack.Count - 1);
                    Main.m_Procedure.SwitchProcedure(ProcedureStack[ProcedureStack.Count - 1]);
                }
            });
            return;
        }
        ProcedureStack.RemoveAt(ProcedureStack.Count - 1);
        Main.m_Procedure.SwitchProcedure(ProcedureStack[ProcedureStack.Count - 1]);
    }

    public void ContinueExp()
    {
        Main.m_Procedure.SwitchProcedure(ProcedureStack[ProcedureStack.Count - 1]);
    }

    private void StartNewExp()
    {
        RecordManager.ClearTempRecord();
        Main.m_Procedure.SwitchProcedure<ChooseExpProcedure>();
        ProcedureStack.Clear();
        ProcedureStack.Add(typeof(StartProcedure));
        ProcedureStack.Add(typeof(ChooseExpProcedure));
    }

    private void Start()
    {
        Debug.Log(ProcedureStack.Count);
        if (ProcedureStack.Count == 0)
            ProcedureStack.Add(typeof(StartProcedure));
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
    }

    private void AddValue()
    {
        if (ValueValidator.ValidateQuantities(RecordManager.tempRecord.quantities))
        {
            Main.m_Procedure.SwitchProcedure<EnterExpressionProcedure>();
            ProcedureStack.Add(typeof(EnterExpressionProcedure));
        }
    }

    private void EnterExpression()
    {
        Main.m_Procedure.SwitchProcedure<PreviewProcedure>();
        ProcedureStack.Add(typeof(PreviewProcedure));
    }

    private void PreviewConfirm()
    {
        Main.m_Procedure.SwitchProcedure<EnterClassroomProcedure>();
        ProcedureStack.Add(typeof(EnterClassroomProcedure));
    }

    public override void OnDestroy()
    {
        RecordManager.tempRecord.Save();
    }
}
