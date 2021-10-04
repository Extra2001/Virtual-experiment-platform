/************************************************************************************
    作者：荆煦添
    描述：物理量校验合法性
*************************************************************************************/
using System.Collections.Generic;
using System.Linq;

public class ValueValidator
{
    public static bool ValidateQuantities(List<QuantityModel> models)
    {
        if (models.Count < 1)
        {
            ShowModel("请添加至少1个物理量");
            return false;
        }
        foreach (var item in models)
        {
            if (models.Where(x => x.Symbol.Equals(item.Symbol) || x.Name.Equals(item.Name)).Count() > 1)
            {
                ShowModel("物理量名称或符号存在重复");
                return false;
            }
        }
        foreach (var item in models)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                ShowModel($"请填写第{models.FindIndex(x => x.Equals(item)) + 1}个物理量的名称");
                return false;
            }
            if (string.IsNullOrEmpty(item.Symbol))
            {
                ShowModel($"请填写物理量\"{item.Name}\"计算符号");
                return false;
            }
            if (!StaticMethods.ValidVarname(item.Symbol))
            {
                ShowModel($"物理量\"{item.Name}\"的计算符号\"{item.Symbol}\"不合法");
                return false;
            }
        }
        return true;
    }

    private static void ShowModel(string message)
    {
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Message = message,
            ShowCancel = false
        });
    }
}
