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

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(CreateWithDestory);
    }

    /// <summary>
    /// 生成主程序
    /// </summary>
    public static GameObject Create(ObjectsModel model)
    {
        if (model == null) return null;
        bool setPosition = true;
        ObjectValue objectValue;
        // 生成物体
        RecordManager.tempRecord.showedObject = model;
        var objLoader = new OBJLoader();
        var obj = objLoader.Load(model.ResourcePath);
        print(model);
        // 挂载组件
        if ((objectValue = obj.GetComponent<ObjectValue>()) == null)
            (objectValue = obj.AddComponent<ObjectValue>()).ObjectModel = model;
        else
        {
            setPosition = false;
            obj.transform.position = objectValue.Position;
            obj.transform.rotation = objectValue.Rotation;
        }
        // 遍历计算点
        Vector3 ClosestPoint = new Vector3();
        Vector3 FarthestPoint = new Vector3();
        int cnt = 0;
        foreach (Transform item in obj.transform)
        {
            
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
                rigid.drag=10;
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
            item.gameObject.tag = ("Mesured");
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
        if (setPosition)
        {
            var rec = RecordManager.tempRecord;
            // 计算位置
            objectValue.Position = new Vector3(rec.objectStartPosition[0],
                rec.objectStartPosition[1] + objectValue.BaseSize.y * scale / 2, rec.objectStartPosition[2]);
        }
        return obj;
    }

    /// <summary>
    /// 重新计算模型中心点
    /// </summary>
    static void ResetModelPivot(GameObject Model)
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
        {
            ShowedGameObject = Create(RecordManager.tempRecord.showedObject);
        }           
    }
    /// <summary>
    /// 生成新物体前销毁旧物体
    /// </summary>
    private void CreateWithDestory()
    {
        if (ShowedGameObject != null)
        {
            foreach (Transform item in ShowedGameObject.transform)
                item.gameObject.SetActive(false);
            ShowedGameObject.SetActive(false);
        }
        ShowedGameObject = Create(objects);

        Main.m_UI.CloseUI<BagControl>();
    }
    /// <summary>
    /// 销毁已生成的物体
    /// </summary>
    public static void DestroyObjecthh()
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
