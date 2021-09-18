using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComplexDataProcessNextBack : HTBehaviour
{
    public Button BackButton;
    public Button NextButton;

    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(Back);
        NextButton.onClick.AddListener(Next);
    }

    public void Back()
    {
        GameManager.Instance.SwitchProcedure<MeasuredDataProcessProcedure>();
    }
    public void Next()
    {
        bool flag;
        string reason;
        (flag, reason) = StaticMethods.CheckUncertain(RecordManager.tempRecord.complexQuantityModel.AnswerAverage, RecordManager.tempRecord.complexQuantityModel.AnswerUncertain);

        if (flag)
        {
            GameManager.Instance.SwitchProcedure<ProcessResultProcedure>();
        }
        else
        {
            RecordManager.tempRecord.score.ComplexQuantityError += 1;
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                ShowCancel = false,
                Message = new BindableString(reason)
            });
        }
    }
}
