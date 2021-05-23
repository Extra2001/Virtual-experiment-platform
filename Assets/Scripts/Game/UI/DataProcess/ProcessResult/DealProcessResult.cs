using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealProcessResult : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public GameObject PerfectTips;
    public GameObject MistakeTips;
    public GameObject WrongFormula;
    public GameObject RightFormula;

    List<DataChart> dataChart = new List<DataChart>();

    // Start is called before the first frame update
    void Start()
    {
        
    
    }

    private void DealData()
    {
        double avg, ua, u;
        for (int i = 0; i < RecordManager.tempRecord.quantities.Count; i++)
        {
            dataChart.Add(new DataChart());
            dataChart[i].Ub = Main.m_Entity.GetEntities(RecordManager.tempRecord.quantities[i].InstrumentType)[0].Cast<InstrumentBase>().ErrorLimit.ToString();
            (avg, ua, u) = StaticMethods.CalcUncertain(RecordManager.tempRecord.quantities[i].Data, double.Parse(dataChart[i].Ub));
            dataChart[i].Name = RecordManager.tempRecord.quantities[i].Symbol;
            dataChart[i].Average = avg.ToString();
            dataChart[i].Uncertain = u.ToString();
            dataChart[i].Ua = ua.ToString();
        }
    }

    public void Show()
    {
        DealData();

        if (false)
        {
            //如果前无错
            PerfectTips.SetActive(true);
            MistakeTips.SetActive(false);
        }
        else
        {
            PerfectTips.SetActive(false);
            MistakeTips.SetActive(true);
            DealMistake();
        }
    }

    private void DealMistake()
    {
        RightFormula.GetComponent<FormulaController>().LoadFormula(Constants.FormulaUncertaintyA);
    }


}
