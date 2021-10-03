/************************************************************************************
    作者：荆煦添
    描述：物理量校验合法性
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;

public class ValueValidator
{
    public static bool ValidateQuantities(List<QuantityModel> models)
    {
        if (models.Count < 1)
        {
            UIAPI.Instance.ShowModel(new SimpleModel()
            {
                Message = "请添加至少1个物理量",
                ShowCancel = false
            });
            return false;
        }
        // 相同的逻辑有待添加。
        foreach (var item in models)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Message = $"请填写第{models.FindIndex(x => x.Equals(item)) + 1}个物理量的名称",
                    ShowCancel = false
                });
                return false;
            }
            if (string.IsNullOrEmpty(item.Symbol))
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Message = $"请填写物理量\"{item.Name}\"计算符号",
                    ShowCancel = false
                });
                return false;
            }
            if (!StaticMethods.ValidVarname(item.Symbol))
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    Message = $"物理量\"{item.Name}\"的计算符号\"{item.Symbol}\"不合法",
                    ShowCancel = false
                });
                return false;
            }
        }
        return true;
    }
}
