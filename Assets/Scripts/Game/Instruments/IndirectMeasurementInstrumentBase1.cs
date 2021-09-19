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
            // 步进增加
        });
        infoItems["_StepSub"].onValueChangedDouble.Add(step =>
        {
            // 步进减少
        });
        infoItems["_ConfirmButton"].onValueChanged.Add(() =>
        {
            double re = Convert.ToDouble(infoItems["_RandomError"].GameObject.GetComponent<InputField>().text);
            double mainValue = Convert.ToDouble(infoItems["_MainValue"].GameObject.GetComponent<InputField>().text);
            if (re > ErrorLimit)
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString("随机误差不能大于仪器误差限")
                });
            else if (mainValue > URV || mainValue < LRV)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString("主值不能超过量程")
                });
            }
            else
            {
                RandomErrorLimit = re;
                MainValue = mainValue;
                ShowValue(mainValue);
            }
        }); 

    }

    public abstract void ReshowValue();//主值不变，随机误差变化重新生成读数


}
