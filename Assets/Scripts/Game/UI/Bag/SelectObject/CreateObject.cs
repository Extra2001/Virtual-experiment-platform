/************************************************************************************
    作者：张峻凡、荆煦添
    描述：生成被测物体脚本
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using UnityEngine.UI;
using Dummiesman;

public class CreateObject : HTBehaviour
{
    public ObjectsModel objects = null;
    private static GameObject ShowedGameObject = null;

    /// <summary>
    /// 生成主程序
    /// </summary>
    public static void Create(ObjectsModel model)
    {
        if (model == null) return;
        RecordManager.tempRecord.showedObject = model;
        if (model.Integrated)
            CreatePrefab();
        else
            CreateObj();
    }
    private static void Create(GameObject obj)
    {
        var model = RecordManager.tempRecord.showedObject;
        ObjectValue objectValue;
        // 挂载组件
        if ((objectValue = obj.GetComponent<ObjectValue>()) == null)
            (objectValue = obj.AddComponent<ObjectValue>()).ObjectModel = model;
        // 遍历计算点
        Vector3 ClosestPoint = new Vector3();
        Vector3 FarthestPoint = new Vector3();
        int cnt = 0;
        foreach (Transform item in obj.transform)
        {
            // 挂载组件
            if (item.gameObject.GetComponent<MeshCollider>() == null)
            {
                var meshCollider = item.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.isTrigger = false;
            }
            ResetModelPivot(item.gameObject);
            if (item.gameObject.GetComponent<Rigidbody>() == null)
            {
                var rigid = item.gameObject.AddComponent<Rigidbody>();
                rigid.useGravity = false;
                rigid.drag = 10;
                rigid.mass = 10;
                rigid.angularDrag = 10;
                rigid.isKinematic = false;
            }
            if (item.gameObject.GetComponent<SelfRotate>() == null)
            {
                var selfRotate = item.gameObject.AddComponent<SelfRotate>();
            }
            if (item.gameObject.GetComponent<RightButtonObject>() == null)
            {
                if (model.childrenPostition.Count == cnt)
                    model.childrenPostition.Add(item.localPosition.GetMyVector());
                else if (model.childrenPostition.Count > cnt)
                    item.localPosition = model.childrenPostition[cnt];

                if (model.childrenRotation.Count == cnt)
                    model.childrenRotation.Add(item.localRotation.GetMyVector());
                else if (model.childrenRotation.Count > cnt)
                    item.localRotation = model.childrenRotation[cnt];

                var right = item.gameObject.AddComponent<RightButtonObject>();
                right.objectValue = objectValue;
                right.index = cnt;
            }
            // 设置Tag和Layer
            item.gameObject.tag = "Mesured";
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
            cnt++;
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
        objectValue.Position = new Vector3(rec.objectStartPosition[0],
            rec.objectStartPosition[1] + objectValue.BaseSize.y * scale / 2, rec.objectStartPosition[2]);
        ShowedGameObject = obj;

        if (!model.position.Equals(new MyVector3()))
            obj.transform.position = model.position;
        if (!model.rotation.Equals(new MyVector4()))
            obj.transform.rotation = model.rotation;
    }
    private static void CreateObj()
    {
        var model = RecordManager.tempRecord.showedObject;
        var objLoader = new OBJLoader();
        Create(objLoader.Load(model.ResourcePath));
    }
    private static void CreatePrefab()
    {
        var model = RecordManager.tempRecord.showedObject;
        Main.m_Resource.LoadPrefab(new PrefabInfo(null, null, model.ResourcePath), null, loadDoneAction: x =>
            Create(x));
    }
    /// <summary>
    /// 重新计算模型中心点
    /// </summary>
    private static void ResetModelPivot(GameObject Model)
    {
        //获得模型的中心
        Vector3 center = Model.GetComponent<MeshCollider>().sharedMesh.bounds.center;

        Mesh mesh = Model.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        //网格顶点是本地坐标
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= center;
        }

        mesh.vertices = vertices;

        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        Model.GetComponent<MeshFilter>().mesh = mesh;

        Destroy(Model.GetComponent<MeshCollider>());
        var meshCollider = Model.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.isTrigger = false;
    }
    /// <summary>
    /// 生成存档中保存的物体
    /// </summary>
    public static void CreateRecord()
    {
        if (RecordManager.tempRecord.showedObject != null && ShowedGameObject == null)
            Create(RecordManager.tempRecord.showedObject);
    }
    /// <summary>
    /// 隐藏现有仪器
    /// </summary>
    public static void HideCurrent()
    {
        if (ShowedGameObject != null)
        {
            foreach (Transform item in ShowedGameObject.transform)
                item.gameObject.SetActive(false);
            ShowedGameObject.SetActive(false);
        }
        ShowedGameObject = null;
    }
}
