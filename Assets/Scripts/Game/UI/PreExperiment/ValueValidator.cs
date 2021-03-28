using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueValidator
{
    public static bool ValidateQuantities(List<QuantityModel> models)
    {
        return true;
        if (models.Count < 2)
        {
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                Message = new BindableString("请添加至少2个物理量"),
                ShowCancel = false
            });
            return false;
        }
        // 相同的逻辑有待添加。
        foreach(var item in models)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    Message = new BindableString("请填写物理量名称"),
                    ShowCancel = false
                });
                return false;
            }
            if (string.IsNullOrEmpty(item.Symbol))
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    Message = new BindableString("请填写物理量计算符号"),
                    ShowCancel = false
                });
                return false;
            }
        }
        return true;
    }
}
