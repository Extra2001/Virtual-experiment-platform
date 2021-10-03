using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasuredGraphic1 : HTBehaviour
{
    private QuantityModel quantity;

    public void Show(QuantityModel quantity)
    {
        this.quantity = quantity;
    }

    public bool CheckAll(bool silent = false)
    {
        //������������Ƿ���ȷ
        if (quantity.IndependentData.data.Count != quantity.MesuredData.data.Count)
        {
            if (!silent)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString("���������������ȷ")
                });
            }

            return false;
        }

        return true;
    }
}
