/************************************************************************************
    作者：荆煦添
    描述：物理量单元格的处理程序
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.UI.Extensions;

public class QuantityCell : HTBehaviour
{
    public InputField _Name;
    public InputField _Symbol;
    public Dropdown _Instrument;
    public Stepper _Group;
    public Text _GroupIndicator;
    public Text _Range;
    public Text _Unit;
    public Button _Delete;

    private List<InstrumentBase> instruments = new List<InstrumentBase>();

    public QuantityModel Quantity { get => GetQuantity(); set => SetQuantity(value); }

    private QuantityModel QuantityReference = null;
    /// <summary>
    /// 显示物理量
    /// </summary>
    /// <param name="model"></param>
    private void SetQuantity(QuantityModel model)
    {
        instruments.Clear();
        foreach (var item in CommonTools.GetSubClassNames(typeof(InstrumentBase)).Where(x => !x.IsAbstract))
        {
            var i = item.CreateInstrumentInstance();
            instruments.Add(i);
        }
        _Instrument.ClearOptions();
        _Instrument.AddOptions(instruments.Select(x => x.InstName).ToList());

        _Name.text = model.Name;
        _Symbol.text = model.Symbol;
        int index = instruments.FindIndex(x => x.GetType() == model.InstrumentType);
        index = index == -1 ? 0 : index;
        var inst = instruments[index];
        _Instrument.value = index;
        _Group.value = model.Groups;
        _Range.text = $"量程：{inst.URV - inst.LRV}";
        _Unit.text = $"最小分度值单位：{inst.Unit}";

        QuantityReference = model;
    }
    /// <summary>
    /// 同步物理量
    /// </summary>
    public void SyncQuantity()
    {
        _GroupIndicator.text = _Group.value.ToString();
        QuantityReference.Groups = _Group.value;
        QuantityReference.InstrumentType = instruments[_Instrument.value].GetType();
        QuantityReference.Symbol = _Symbol.text;
        QuantityReference.Name = _Name.text;
    }
    /// <summary>
    /// 获取当前单元格物理量的Model
    /// </summary>
    /// <returns></returns>
    private QuantityModel GetQuantity()
    {
        SyncQuantity();
        return new QuantityModel()
        {
            Name = _Name.text,
            Groups = _Group.value,
            InstrumentType = instruments[_Instrument.value].GetType(),
            Symbol = _Symbol.text
        };
    }
}
