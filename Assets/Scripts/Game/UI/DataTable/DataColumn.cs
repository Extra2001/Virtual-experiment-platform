/************************************************************************************
    作者：荆煦添
    描述：数据记录表格的列
*************************************************************************************/
using HT.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataColumn : HTBehaviour
{
    public DataInput _dataInput;
    public GameObject _Content;
    public Button _DeleteButton;
    public DRDropDown _Dropdown;
    public Button _AddButton;
    [NonSerialized]
    public DataTable dataTable;

    private DataColumnType type;
    private DataColumnModel dataColumnModel;
    private List<DataColumnModel> datas = new List<DataColumnModel>();
    private List<DataInput> showedInputs = new List<DataInput>();
    private bool single = false;

    public bool ReadOnly
    {
        get => showedInputs.FirstOrDefault() != null && showedInputs.FirstOrDefault().ReadOnly;
        set { showedInputs.ForEach(x => { x.ReadOnly = value; x.Deletable = !value; }); _AddButton.gameObject.SetActive(!value); }
    }
    public bool Changable
    {
        get => _Dropdown.interactable;
        set => _Dropdown.interactable = value;
    }
    public bool Deletable
    {
        get => _DeleteButton.gameObject.activeSelf;
        set
        {
            SetSize(value ? 2 : 1);
            _DeleteButton.gameObject.SetActive(value);
        }
    }

    private void Start()
    {
        _Dropdown.onValueChanged.AddListener(x =>
        {
            Show(datas[x], single);
            dataTable.RefreshAllDropdown();
        });
        _DeleteButton.onClick.AddListener(() =>
        {
            if (Deletable)
            {
                dataColumnModel.addedToTable = false;
                dataTable.DeleteColumn(this);
            }
        });
        _AddButton.onClick.AddListener(AddInput);
    }

    public void SetClass(DataColumnType type, bool init = false)
    {
        this.type = type;
        if (type == DataColumnType.Mesured)
        {
            datas.Clear();
            foreach (var item in RecordManager.tempRecord.quantities)
            {
                if (item.MesuredData == null)
                    item.MesuredData = new DataColumnModel()
                    {
                        name = $"[原] {item.Name} ({item.InstrumentType.CreateInstrumentInstance().UnitSymbol})",
                        quantitySymbol = item.Symbol,
                        type = DataColumnType.Mesured
                    };
                datas.Add(item.MesuredData);
            }
        }
        else if (type == DataColumnType.Independent)
        {
            datas.Clear();
            foreach (var item in RecordManager.tempRecord.quantities)
            {
                if (item.IndependentData == null)
                    item.IndependentData = new DataColumnModel()
                    {
                        name = $"[步] {item.Name} ({item.InstrumentType.CreateInstrumentInstance().UnitSymbol})",
                        quantitySymbol = item.Symbol,
                        type = DataColumnType.Independent
                    };
                datas.Add(item.IndependentData);
            }
        }
        else if (type == DataColumnType.Differenced)
        {
            datas.Clear();
            foreach (var item in RecordManager.tempRecord.quantities)
            {
                if (item.DifferencedData == null)
                    item.DifferencedData = new DataColumnModel()
                    {
                        name = $"[逐] {item.Name} ({item.InstrumentType.CreateInstrumentInstance().UnitSymbol})",
                        quantitySymbol = item.Symbol,
                        type = DataColumnType.Differenced
                    };
                datas.Add(item.DifferencedData);
            }
        }
        _Dropdown.ClearOptions();
        _Dropdown.AddOptions(datas.Select(x => x.name).ToList());
        _Dropdown.disables.Clear();
        for (int i = 0; i < datas.Count; i++)
            if (datas[i].addedToTable)
                _Dropdown.disables.Add(i);
        if (init) Show(datas.Where(x => !x.addedToTable).FirstOrDefault(), single);
    }

    public void SetSize(int size)
    {
        var sized = _Dropdown.rectTransform().sizeDelta;
        if (size == 1) sized.x = 200;
        else if (size == 2) sized.x = 170;
        _Dropdown.rectTransform().sizeDelta = sized;
        var position = _Dropdown.transform.localPosition;
        if (size == 1) position.x = 0;
        else if (size == 2) position.x = -15;
        _Dropdown.transform.localPosition = position;
    }

    public void RefreshDropdown()
    {
        _Dropdown.disables.Clear();
        for (int i = 0; i < datas.Count; i++)
            if (datas[i].addedToTable)
                _Dropdown.disables.Add(i);
    }

    public void Show(DataColumnModel dataColumnModel, bool single = false)
    {
        this.single = single;
        if (!single)
        {
            if (this.dataColumnModel != null)
                this.dataColumnModel.addedToTable = false;
            dataColumnModel.addedToTable = true;
            SetClass(type);
        }
        this.dataColumnModel = dataColumnModel;
        foreach (var item in showedInputs)
            Destroy(item.gameObject);
        showedInputs.Clear();
        _Dropdown.SetValueWithoutNotify(datas.FindIndex(x => x.name.Equals(dataColumnModel.name)));
        for (int i = 0; i < dataColumnModel.data.Count; i++)
            showedInputs.Add(InstantiateDataInput(i));
        _AddButton.transform.SetAsLastSibling();
        if (dataTable != null)
            dataTable.RefreshAllDropdown();
    }

    public void AddInput()
    {
        dataColumnModel.data.Add("");
        Show(dataColumnModel, single);
    }

    public void DeleteInput(DataInput dataInput)
    {
        dataColumnModel.data.RemoveAt(dataInput.Index);
        Show(dataColumnModel, single);
    }

    private DataInput InstantiateDataInput(int index)
    {
        var ret = Instantiate(_dataInput, _Content.transform);
        ret.Show(dataColumnModel, index);
        ret.dataColumn = this;
        return ret;
    }
}
