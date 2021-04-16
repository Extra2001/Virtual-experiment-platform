using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;

public class EnterUncertaintyProcedure : ProcedureBase
{
    public override void OnInit()
    {
		base.OnInit();
    }

    public override void OnEnter(ProcedureBase lastProcedure)
    {
        GameManager.Instance.ShowUncertainty();
        base.OnEnter(lastProcedure);
    }

    public void ShowUncertainty(QuantityModel quantity)
    {
        Main.m_UI.CloseUI<UncertaintyUILogic>();
        Main.m_UI.OpenResidentUI<UncertaintyUILogic>(quantity);
    }

    public override void OnLeave(ProcedureBase nextProcedure)
    {
        base.OnLeave(nextProcedure);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnUpdateSecond()
    {
        base.OnUpdateSecond();
    }
}