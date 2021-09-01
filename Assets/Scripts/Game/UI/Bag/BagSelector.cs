using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Threading.Tasks;

public class BagSelector : HTBehaviour
{
    public BagItem BagItem;
    public GameObject BagItemRoot;
    public Button InstrumentButton;
    public Button ObjectButton;
    public Button HistoryButton;

    public InstrumentPanel InstrumentPanel;
    public ObjectsPanel ObjectPanel;
    public HistorysPanel HistoryPanel;

    private Button CurrentActiveButton = null;

    private void Start()
    {
        InstrumentButton.onClick.AddListener(() =>
        {
            if (CurrentActiveButton != InstrumentButton)
            {
                ChangeButtonColor(InstrumentButton);
                LoadInstruments();
            }
        });
        ObjectButton.onClick.AddListener(() =>
        {
            if (CurrentActiveButton != ObjectButton)
            {
                ChangeButtonColor(ObjectButton);
                LoadObjects();
            }
        });
        HistoryButton.onClick.AddListener(() =>
        {
            if (CurrentActiveButton != HistoryButton)
            {
                ChangeButtonColor(HistoryButton);
                LoadHistorys();
            }
        });

        ChangeButtonColor(InstrumentButton);
        LoadInstruments();
        StartCoroutine(nameof(PreLoadImages));
    }

    public void Show()
    {
        UIShowHideHelper.ShowFromButtom(gameObject);
    }

    public void Hide()
    {
        UIShowHideHelper.HideToButtom(gameObject);
    }

    public void GenerateInstrument(InstrumentPanel panel)
    {
        CreateInstrument.HideCurrent();
        CreateInstrument.Create(panel.instrumentType);
    }

    public void GenerateObjects(ObjectsPanel panel)
    {
        CreateObject.HideCurrent();
        CreateObject.Create(panel.objectsModel.DeepCopy<ObjectsModel>());
    }

    public void GenerateHistory(HistorysPanel panel)
    {
        CreateInstrument.HideCurrent();
        CreateInstrument.Create(panel.model);
    }

    public void SelectInstrument(BagItem bagItem)
    {
        ShowPanel(bagItem);
        InstrumentPanel.gameObject.SetActive(true);
        InstrumentPanel.SetData(bagItem.instrumentType);
    }

    public void SelectObject(BagItem bagItem)
    {
        ShowPanel(bagItem);
        ObjectPanel.gameObject.SetActive(true);
        ObjectPanel.SetData(bagItem.objectsModel);
    }

    public void SelectHistory(BagItem bagItem)
    {
        ShowPanel(bagItem);
        HistoryPanel.gameObject.SetActive(true);
        HistoryPanel.SetData(bagItem.instrumentInfoModel);
    }

    private IEnumerator PreLoadImages()
    {
        var list = GameManager.Instance.objectsModels;
        foreach (var item in list)
        {
            yield return CommonTools.GetBytes(item.PreviewImage);
            yield return CommonTools.GetSprite(item.PreviewImage);
        }
    }

    private void ShowPanel(BagItem bagItem)
    {
        InstrumentPanel.gameObject.SetActive(false);
        ObjectPanel.gameObject.SetActive(false);
        HistoryPanel.gameObject.SetActive(false);
        ChangeItemButtonColor(bagItem);
    }

    private void LoadInstruments()
    {
        ClearBagMenu();
        var list = CommonTools.GetSubClassNames(typeof(InstrumentBase)).Where(x => !x.IsAbstract).ToList();
        var instances = new List<BagItem>();
        foreach (var item in list)
        {
            instances.Add(Instantiate(BagItem, BagItemRoot.transform));
            instances.Last().SetData(this, item);
        }
        SelectInstrument(instances.First());
    }

    private void LoadObjects()
    {
        ClearBagMenu();
        var list = GameManager.Instance.objectsModels;
        var instances = new List<BagItem>();
        foreach (var item in list)
        {
            instances.Add(Instantiate(BagItem, BagItemRoot.transform));
            instances.Last().SetData(this, item);
        }
        SelectObject(instances.First());
    }

    private void LoadHistorys()
    {
        ClearBagMenu();
        var list = RecordManager.tempRecord.historyInstrument;
        list.Reverse();
        foreach (var item in list)
            Instantiate(BagItem, BagItemRoot.transform).SetData(this, item);
        SelectHistory(BagItemRoot.transform.GetComponentInChildren<BagItem>());
    }

    private void ClearBagMenu()
    {
        foreach (Transform item in BagItemRoot.transform)
            Destroy(item.gameObject);
    }

    private void ChangeItemButtonColor(BagItem button)
    {
        if (BagItemRoot.transform.childCount == 0) return;
        var colorBlock = BagItemRoot.transform.GetComponentInChildren<Button>().colors;
        colorBlock.normalColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        colorBlock.highlightedColor = new Color(245 / 255f, 245 / 255f, 245 / 255f, 255 / 255f);
        colorBlock.selectedColor = new Color(245 / 255f, 245 / 255f, 245 / 255f, 255 / 255f);
        foreach (Transform item in BagItemRoot.transform)
            item.GetComponent<Button>().colors = colorBlock;

        colorBlock.normalColor = new Color(0, 101 / 255f, 195 / 255f, 255 / 255f);
        colorBlock.highlightedColor = new Color(101 / 255f, 195 / 255f, 255 / 255f, 255 / 255f);
        colorBlock.selectedColor = new Color(101 / 255f, 195 / 255f, 255 / 255f, 255 / 255f);
        button.GetComponent<Button>().colors = colorBlock;
    }

    private void ChangeButtonColor(Button button)
    {
        CurrentActiveButton = button;

        var colorBlock = InstrumentButton.colors;
        colorBlock.normalColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);

        InstrumentButton.colors = colorBlock;
        ObjectButton.colors = colorBlock;
        HistoryButton.colors = colorBlock;

        colorBlock.normalColor = new Color(0, 101 / 255f, 195 / 255f, 255 / 255f);
        button.colors = colorBlock;
    }
}