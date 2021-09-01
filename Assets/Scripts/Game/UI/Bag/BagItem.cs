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

    public BagSelector bagSelector;
    public Type instrumentType = null;
    public ObjectsModel objectsModel = null;
    public InstrumentInfoModel instrumentInfoModel = null;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (instrumentType != null)
                bagSelector.SelectInstrument(this);
            else if (objectsModel != null)
                bagSelector.SelectObject(this);
            else if (instrumentInfoModel != null)
                bagSelector.SelectHistory(this);
        });
    }

    public void SetData(BagSelector selector, Type instrumentType)
    {
        this.bagSelector = selector;
        this.instrumentType = instrumentType;
        var instance = instrumentType.CreateInstrumentInstance();

        Title.text = instance.InstName;
        Image.sprite = instance.previewImage;
    }

    public void SetData(BagSelector selector, ObjectsModel objectsModel)
    {
        this.bagSelector = selector;
        this.objectsModel = objectsModel;

        Title.text = objectsModel.Name;
        Image.sprite = CommonTools.GetSprite(objectsModel.PreviewImage);
    }

    public void SetData(BagSelector selector, InstrumentInfoModel instrumentInfoModel)
    {
        this.bagSelector = selector;
        this.instrumentInfoModel = instrumentInfoModel;
        var instance = instrumentInfoModel.instrumentType.CreateInstrumentInstance();

        Title.text = instance.InstName;
        Image.sprite = instance.previewImage;
    }
}
