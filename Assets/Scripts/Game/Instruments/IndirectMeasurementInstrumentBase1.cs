/************************************************************************************
    作者：张峻凡
    描述：复杂仪器的基类
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public abstract class IndirectMeasurementInstrumentBase : InstrumentBase
{
    public override void ShowInfoPanel(Dictionary<string, IntrumentInfoItem> infoItems)//读数类仪器添加手动设置主值和随机误差的功能
    {
        base.ShowInfoPanel(infoItems);
        infoItems["_MainValue"].GameObject.SetActive(true);
        infoItems["_MainValue"].GameObject.GetComponent<InputField>().text = MainValue.ToString();
        infoItems["_RandomError"].GameObject.SetActive(true);
        infoItems["_RandomError"].GameObject.GetComponent<InputField>().text = RandomErrorLimit.ToString();
        infoItems["_ConfirmButton"].GameObject.SetActive(true);
        infoItems["_StepAdd"].GameObject.SetActive(true);
        infoItems["_StepSub"].GameObject.SetActive(true);
        infoItems["_Step"].GameObject.SetActive(true);
        infoItems["_StepAdd"].onValueChangedDouble.Add(step =>
        {
            infoItems["_MainValue"].GameObject.GetComponent<InputField>().text = (MainValue + step).ToString();
            ShowValue(step + MainValue);
        });
        infoItems["_StepSub"].onValueChangedDouble.Add(step =>
        {
            infoItems["_MainValue"].GameObject.GetComponent<InputField>().text = (MainValue - step).ToString();
            ShowValue(MainValue - step);
        });
        infoItems["_ConfirmButton"].onValueChanged.Add(() =>
        {
            double re = Convert.ToDouble(infoItems["_RandomError"].GameObject.GetComponent<InputField>().text);
            double mainValue = Convert.ToDouble(infoItems["_MainValue"].GameObject.GetComponent<InputField>().text);
            if (re > ErrorLimit)
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = "随机误差不能大于仪器误差限"
                });
            else
            {
                RandomErrorLimit = re;
                ShowValue(mainValue);
            }
        });
    }

    public abstract void ReshowValue();//主值不变，随机误差变化重新生成读数
}
