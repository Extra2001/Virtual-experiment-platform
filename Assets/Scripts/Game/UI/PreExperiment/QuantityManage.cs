using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuantityManage : HTBehaviour
{
    public GameObject ValuePanel;
    public GameObject AddNewPanel;

    private List<GameObject> gameObjects = new List<GameObject>();

    private void Start()
    {
        AddNewPanel.GetComponent<Button>().onClick.AddListener(() =>
        {
            RecordManager.tempRecord.quantities.Add(new QuantityModel());
            LoadQuantities();
        });
        LoadQuantities();
    }

    private void Update()
    {
        foreach(var item in gameObjects)
        {
            item.GetComponent<QuantityCell>().SyncQuantity();
        }
    }

    public void LoadQuantities()
    {
        foreach (var item in gameObjects)
            Destroy(item);
        gameObjects.Clear();
        foreach (var item in RecordManager.tempRecord.quantities)
        {
            var hh = Instantiate(ValuePanel, transform);
            gameObjects.Add(hh);
            hh.GetComponent<QuantityCell>().Quantity = item;
            hh.GetComponent<QuantityCell>()._Delete.onClick.AddListener(() =>
            {
                RecordManager.tempRecord.quantities.Remove(item);
                LoadQuantities();
            });
        }

        AddNewPanel.transform.SetAsLastSibling();
    }
}
