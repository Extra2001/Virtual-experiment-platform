/************************************************************************************
    作者：荆煦添
    描述：背包的项目UI逻辑
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BagItem : HTBehaviour
{
    public Text Title;
    public Image Image;
    public Button Delete;

    [NonSerialized]
    public BagSelector bagSelector;
    public Type instrumentType = null;
    [NonSerialized]
    public ObjectsModel objectsModel = null;
    [NonSerialized]
    public InstrumentInfoModel instrumentInfoModel = null;

    private int working = 0;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (working == 1)
                bagSelector.SelectInstrument(this);
            else if (working == 2)
                bagSelector.SelectObject(this);
            else if (working == 3)
                bagSelector.SelectHistory(this);
        });
        Delete.onClick.AddListener(() => bagSelector.DeleteObject(this));
    }

    public void SetData(BagSelector selector, Type instrumentType)
    {
        working = 1;
        this.Delete.gameObject.SetActive(false);
        this.bagSelector = selector;
        this.instrumentType = instrumentType;
        var instance = instrumentType.CreateInstrumentInstance();

        Title.text = instance.InstName;
        Image.sprite = instance.previewImage;
    }

    public void SetData(BagSelector selector, ObjectsModel objectsModel)
    {
        working = 2;
        this.Delete.gameObject.SetActive(false);
        this.bagSelector = selector;
        this.objectsModel = objectsModel;
        if (!objectsModel.Integrated)
            this.Delete.gameObject.SetActive(true);

        Title.text = objectsModel.Name;
        Image.sprite = CommonTools.GetSprite(objectsModel.PreviewImage);
    }

    public void SetData(BagSelector selector, InstrumentInfoModel instrumentInfoModel)
    {
        working = 3;
        this.Delete.gameObject.SetActive(false);
        this.bagSelector = selector;
        this.instrumentInfoModel = instrumentInfoModel;
        var instance = instrumentInfoModel.instrumentType.CreateInstrumentInstance();

        Title.text = instance.InstName;
        Image.sprite = instance.previewImage;
    }
}
