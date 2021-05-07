using HT.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviorManager<GameManager>
{
    List<Type> ProcedureStack { get => RecordManager.tempRecord.procedureStack; }

    public GameObject MyObject = null;

    public bool CanContinue { get => ProcedureStack.Count > 1; }

    public int _currentQuantityIndex { get => RecordManager.tempRecord.currentQuantityIndex; set => RecordManager.tempRecord.currentQuantityIndex = value; }

    public QuantityModel CurrentQuantity
    {
        get => RecordManager.tempRecord.quantities[_currentQuantityIndex];
    }

    public bool FPSable
    {
        get => firstPersonController.gameObject.activeSelf;
        set
        {
            firstPersonController.gameObject.GetComponent<CharacterController>().enabled = value;
            firstPersonController.enabled = value;
        }
    }

    public bool Movable
    {
        get => firstPersonController.m_WalkSpeed > 0.1;
        set
        {
            if (value)
            {
                firstPersonController.m_WalkSpeed = 30;
                firstPersonController.m_RunSpeed = 50;
            }
            else
                firstPersonController.m_RunSpeed = firstPersonController.m_WalkSpeed = 0;
        }
    }

    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPersonController = null;

    private void Start()
    {
        firstPersonController = GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        if (ProcedureStack.Count == 0)
            ProcedureStack.Add(typeof(StartProcedure));
        Main.m_Event.Subscribe<Sitdown>(WhenSitdown);
        Main.m_Event.Subscribe<StartNewExpEventHandler>(StartNewExp);
        Main.m_Event.Subscribe<ChooseExpEventHandler>(ChooseExp);
        Main.m_Event.Subscribe<AddValueEventHandler>(AddValue);
        Main.m_Event.Subscribe<EnterExpressionEventHandler>(EnterExpression);
        Main.m_Event.Subscribe<PreviewConfirmEventHandler>(PreviewConfirm);
        Main.m_Event.Subscribe<ProcessExplainEventHandler>(ProcessTips);
        Main.m_Event.Subscribe<StartUncertaintyEventHandler>(EnterUncertainty);
    }

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
        UIAPI.Instance.ShowAndHideLoading(1000);
        MainThread.Instance.DelayAndRun(500, () =>
        {
            Main.m_Procedure.SwitchProcedure<EnterClassroomProcedure>();
            ProcedureStack.Add(typeof(EnterClassroomProcedure));
        });
    }

    public void SwitchProcedure<T>() where T : ProcedureBase
    {
        Main.m_Procedure.SwitchProcedure<T>();
        ProcedureStack.Add(typeof(T));
    }

    private void ProcessTips()
    {
        Main.m_Procedure.SwitchProcedure<ProcessExplainProcedure>();
        ProcedureStack.Add(typeof(ProcessExplainProcedure));
    }

    public void EnterUncertainty()
    {
        Main.m_Procedure.SwitchProcedure<MeasuredDataProcessProcedure>();
        ProcedureStack.Add(typeof(MeasuredDataProcessProcedure));
        _currentQuantityIndex = 0;
        ShowUncertainty();
    }

    public void ShowUncertainty()
    {
        var pro = Main.m_Procedure.GetProcedure<MeasuredDataProcessProcedure>();
        pro.ShowUncertainty(CurrentQuantity);
    }

    /// <summary>
    /// 自动保存
    /// </summary>
    public override void OnDestroy()
    {
        RecordManager.tempRecord.Save();
    }
}
