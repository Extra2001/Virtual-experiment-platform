using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UncertaintyBackNext : HTBehaviour
{
    public Button NextButton;
    public Button BackButton;

    private void Start()
    {
        GameManager gm = GameManager.Instance;
        Record rec = RecordManager.tempRecord;
        BackButton.onClick.AddListener(() =>
        {
            if (gm._currentQuantityIndex == 0)
            {
                Main.m_UI.CloseUI<MeasuredDataProcess>();
                gm.SwitchBackProcedure();
            }
            else
            {
                gm._currentQuantityIndex--;
                gm.ShowUncertainty();
            }
        });
        NextButton.onClick.AddListener(() =>
        {
            if (gm._currentQuantityIndex >= rec.quantities.Count - 1)
            {
                gm.SwitchProcedure<ComplexDataProcessProcedure>();
            }
            else
            {
                gm._currentQuantityIndex++;
                gm.ShowUncertainty();
            }
        });
    }
}
