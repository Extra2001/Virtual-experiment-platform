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
        //检查数据组数是否正确
        if (quantity.IndependentData.data.Count != quantity.MesuredData.data.Count)
        {
            if (!silent)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString("你的数据组数不正确")
                });
            }

            return false;
        }

        return true;
    }
}
