/************************************************************************************
    作者：荆煦添
    描述：物理量选择单元格
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class QuantitySelectCell : HTBehaviour
{
    public Text _Name;
    public Text _Symbol;
    public QuantityModel Quantity { get => _Quantity; set => SetQuantity(value); }
    private QuantityModel _Quantity = null;

    /// <summary>
    /// 设置物理量信息到单元格
    /// </summary>
    /// <param name="model"></param>
    public void SetQuantity(QuantityModel model)
    {
        _Quantity = model;
        _Name.text = model.Name;
        _Symbol.text = model.Symbol;
        string symbol;
        if (model.processMethod == 2)
            symbol = "Δ" + model.Symbol;
        else if (model.processMethod == 3 || model.processMethod == 4)
            symbol = "k_" + model.Symbol;
        else symbol = model.Symbol;
        _Symbol.text = symbol;
    }
}
