/************************************************************************************
    作者：张峻凡
    描述：计算不确定度的数据显示器
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class UncertaintyInput : HTBehaviour
{
    public Text _Title;
    public Text _AverageTitle;
    public Text _UaTitle;
    public Text _UbTitle;
    public Text _ComplexTitle;

    public DataColumn dataColumn;

    public void Show(QuantityModel quantity)
    {
        var instance = quantity.InstrumentType.CreateInstrumentInstance();

        _Title.text = "处理" + quantity.Name + ":" + quantity.Symbol + "/" + instance.UnitSymbol;
        _AverageTitle.text = quantity.Name + "的平均值\n\n=";
        _UaTitle.text = quantity.Name + "的A类不确定度\n\n=";
        _UbTitle.text = quantity.Name + "的B类不确定度\n\n=";
        _ComplexTitle.text = quantity.Name + "的合成不确定度\n\n=";
    }
}
