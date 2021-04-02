using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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

    private List<Type> GetSubClassNames(Type parentType)
    {
        var subTypeList = new List<Type>();
        var assembly = parentType.Assembly;
        var assemblyAllTypes = assembly.GetTypes();
        foreach (var itemType in assemblyAllTypes)
        {
            var baseType = itemType.BaseType;
            if (baseType != null)
            {
                if (baseType.Name == parentType.Name)
                {
                    subTypeList.Add(itemType);
                }
            }
        }
        return subTypeList;
    }

    private void SetQuantity(QuantityModel model)
    {
        instruments.Clear();
        foreach (var item in GetSubClassNames(typeof(InstrumentBase)))
        {
            var i = (InstrumentBase)Activator.CreateInstance(item);
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

    public void SyncQuantity()
    {
        _GroupIndicator.text = _Group.value.ToString();
        QuantityReference.Groups = _Group.value;
        QuantityReference.InstrumentType = instruments[_Instrument.value].GetType();
        QuantityReference.Symbol = _Symbol.text;
        QuantityReference.Name = _Name.text;
    }

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
