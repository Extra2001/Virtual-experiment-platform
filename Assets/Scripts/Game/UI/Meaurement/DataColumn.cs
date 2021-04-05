using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataColumn : HTBehaviour
{
    public DataInput _dataInput;
    public GameObject _Content;
    public Text _Name;
    public QuantityModel Quantity => quantity;

    private QuantityModel quantity;
    private List<DataInput> showedInputs = new List<DataInput>();

    public void ShowQuantity(QuantityModel quantity)
    {
        foreach (var item in showedInputs)
            Destroy(item.gameObject);
        showedInputs.Clear();
        this.quantity = quantity;

        _Name.text = quantity.Name;

        if (quantity.Data.Count != quantity.Groups)
        {
            quantity.Data.Clear();
            for (int i = 0; i < quantity.Groups; i++)
            {
                quantity.Data.Add(0);
                var showed = Instantiate(_dataInput, _Content.transform);
                showedInputs.Add(showed);
                showed.Show(i + 1);
                showed._Value.onEndEdit.AddListener(_ =>
                {
                    quantity.Data[i] = showed.Value;
                });
            }
        }
        else
        {
            for (int i = 0; i < quantity.Data.Count; i++)
            {
                var showed = Instantiate(_dataInput, _Content.transform);
                showedInputs.Add(showed);
                showed.Show(i + 1, quantity.Data[i]);
                int tmp = i;
                showed._Value.onEndEdit.AddListener(_ =>
                {
                    quantity.Data[tmp] = showed.Value;
                });
            }
        }
    }
}
