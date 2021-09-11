/************************************************************************************
    作者：荆煦添
    描述：物理量预览页面的单元格
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class QuantityPreviewCell : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public Text _Name;
    public Text _Symbol;
    public Text _InstrumentName;
    public Text _InstrumentUnit;

    public void SetQuantity(QuantityModel model)
    {
        _Name.text = model.Name;
        _Symbol.text = model.Symbol;
        var inst = model.InstrumentType.CreateInstrumentInstance();
        _InstrumentName.text = inst.InstName;
        _InstrumentUnit.text = inst.UnitSymbol;
    }
}
