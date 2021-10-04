/************************************************************************************
    作者：荆煦添
    描述：背包根控制器
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.IO;

public class BagSelector : HTBehaviour
{
    
    public BagItem BagItem;
    public GameObject BagItemRoot;
    public Button InstrumentButton;
    public Button ObjectButton;
    public Button HistoryButton;
    public Button ImportButton;

    public InstrumentPanel InstrumentPanel;
    public ObjectsPanel ObjectPanel;
    public HistorysPanel HistoryPanel;

    private Button CurrentActiveButton = null;
    private GameObject PreviewImageCamera;
    private RenderTexture TargetTexture;
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
        ImportButton.onClick.AddListener(ImportObject);
        PreviewImageCamera = GameObject.Find("PreviewImageCamera");

        ChangeButtonColor(InstrumentButton);
        LoadInstruments();
    }

    public void Show()
    {
        UIShowHideHelper.ShowFromButtom(gameObject);
        if (RecordManager.tempRecord.historyInstrument.Where(x => x != null).Count() > 0)
            HistoryButton.interactable = true;
        else
            HistoryButton.interactable = false;
    }

    public void Hide()
    {
        UIShowHideHelper.HideToButtom(gameObject);
    }

    public void GenerateInstrument(InstrumentPanel panel)
    {
        CreateInstrument.HideCurrent();
        CreateInstrument.Create(panel.instrumentType);
        Main.m_UI.GetUI<BagControl>().Hide();
    }

    public void GenerateObjects(ObjectsPanel panel)
    {
        CreateObject.HideCurrent();
        CreateObject.Create(panel.objectsModel.DeepCopy<ObjectsModel>());
        Main.m_UI.GetUI<BagControl>().Hide();
    }

    public void GenerateHistory(HistorysPanel panel)
    {
        CreateInstrument.HideCurrent();
        CreateInstrument.Create(panel.model);
        Main.m_UI.GetUI<BagControl>().Hide();
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

    public void ImportObject()
    {
        if (ImportModel.OpenFile())
        {
            ObjectsModel temp = GameManager.Instance.objectsModels.FirstOrDefault();
            GameObject tempObject = CreateObject.CreateSingleObj(temp.ResourcePath);
            SavePreviewImage(temp.ResourcePath);
            temp.PreviewImage = temp.ResourcePath.Replace(".obj", ".png");
            Destroy(tempObject);

            ChangeButtonColor(ObjectButton);
            LoadObjects();
        }
    }
    
    private void SavePreviewImage(string path)
    {
        PreviewImageCamera.GetComponent<Camera>().enabled = true;
        RenderTexture rt = PreviewImageCamera.GetComponent<Camera>().targetTexture;
        
        PreviewImageCamera.GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        string filepath = path.Replace(".obj", ".png");
        File.WriteAllBytes(filepath, bytes);
        PreviewImageCamera.GetComponent<Camera>().enabled = false;
        Destroy(tex);
    }

    public void DeleteObject(BagItem bagItem)
    {
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Title = "提示",
            Message = $"确认要删除{bagItem.objectsModel.Name}吗？",
            ConfirmAction = () =>
            {
                ImportModel.DeleteModel(bagItem.objectsModel);
                LoadObjects();
            }
        });
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
        var list = CommonTools.GetSubClassNames(typeof(InstrumentBase))
            .Where(x => !x.IsAbstract)
            .OrderBy(x => x.CreateInstrumentInstance().ID).ToList();
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
        var list = RecordManager.tempRecord.historyInstrument.Where(x => x != null);
        list.Reverse();
        var instances = new List<BagItem>();
        foreach (var item in list)
        {
            instances.Add(Instantiate(BagItem, BagItemRoot.transform));
            instances.Last().SetData(this, item);
        }
        SelectHistory(instances.First());
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