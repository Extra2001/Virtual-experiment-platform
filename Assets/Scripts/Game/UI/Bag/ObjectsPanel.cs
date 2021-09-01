using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectsPanel : HTBehaviour
{
    public Image Image;
    public Text Title;
    public Text Detail;
    public Text Integrated;
    public Button Generate;

    public BagSelector selector;
    public ObjectsModel objectsModel;

    private void Start()
    {
        Generate.onClick.AddListener(() =>
        {
            selector.GenerateObjects(this);
        });
    }

    public void SetData(ObjectsModel objectsModel)
    {
        this.objectsModel = objectsModel;

        Image.sprite = CommonTools.GetSprite(objectsModel.PreviewImage);
        Title.text = objectsModel.Name;
        Detail.text = objectsModel.DetailMessage;
        Integrated.text = objectsModel.Integrated ? "内置物体" : "用户导入物体";
    }
}
