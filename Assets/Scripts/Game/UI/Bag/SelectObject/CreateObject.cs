using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Dummiesman;

public class CreateObject : HTBehaviour
{
    public ObjectsModel objects = null;
    private static GameObject ShowedGameObject = null;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(CreateWithDestory);
    }

    public static GameObject Create(ObjectsModel model)
    {
        if (model == null) return null;
        ObjectValue objectValue;
        // 生成物体
        RecordManager.tempRecord.ShowedObject = model;
        var objLoader = new OBJLoader();
        var obj = objLoader.Load(model.ResourcePath);
        // 挂载组件
        if ((objectValue = obj.GetComponent<ObjectValue>()) == null)
            (objectValue = obj.AddComponent<ObjectValue>()).ObjectModel = model;
        // 遍历计算点
        Vector3 ClosestPoint = new Vector3();
        Vector3 FarthestPoint = new Vector3();
        foreach (Transform item in obj.transform)
        {
            if (item.gameObject.GetComponent<MeshCollider>() == null)
            {
                var meshCollider = item.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.isTrigger = true;
            }
            if (item.gameObject.GetComponent<Rigidbody>() == null)
            {
                var rigid = item.gameObject.AddComponent<Rigidbody>();
                rigid.useGravity = false;
                rigid.isKinematic = true;
            }
            // 设置Tag和Layer
            item.gameObject.tag = ("Tools_Be_Moved");
            item.gameObject.layer = 11;
            //获取所有子物体中的最近点和最远点
            var Temp = item.gameObject.GetComponent<MeshFilter>().mesh.bounds.min;
            if (Temp.x < ClosestPoint.x)
                ClosestPoint.x = Temp.x;
            if (Temp.y < ClosestPoint.y)
                ClosestPoint.y = Temp.y;
            if (Temp.z < ClosestPoint.z)
                ClosestPoint.z = Temp.z;

            Temp = item.gameObject.GetComponent<MeshFilter>().mesh.bounds.max;
            if (Temp.x > FarthestPoint.x)
                FarthestPoint.x = Temp.x;
            if (Temp.y > FarthestPoint.y)
                FarthestPoint.y = Temp.y;
            if (Temp.z > FarthestPoint.z)
                FarthestPoint.z = Temp.z;
        }
        // 计算基础大小
        objectValue.BaseSize = new Vector3(Mathf.Abs(FarthestPoint.x - ClosestPoint.x),
            Mathf.Abs(FarthestPoint.y - ClosestPoint.y),
            Mathf.Abs(FarthestPoint.z - ClosestPoint.z));
        // 计算scale
        var max = Mathf.Max(objectValue.BaseSize.x, objectValue.BaseSize.y, objectValue.BaseSize.z);
        var scale = 2f / max;
        objectValue.Scale = scale;
        var rec = RecordManager.tempRecord;
        // 计算位置
        objectValue.Position = new Vector3(rec.ObjectStartPosition[0],
            rec.ObjectStartPosition[1] + objectValue.BaseSize.y * scale / 2, rec.ObjectStartPosition[2]);
        return obj;
    }

    public static void CreateWithoutDestory()
    {
        ShowedGameObject = Create(RecordManager.tempRecord.ShowedObject);
    }

    private void CreateWithDestory()
    {
        if(ShowedGameObject != null)
            Destroy(ShowedGameObject);

        ShowedGameObject = Create(objects);

        Main.m_UI.CloseUI<BagControl>();
    }
}
