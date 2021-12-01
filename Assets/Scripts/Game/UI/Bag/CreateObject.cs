/************************************************************************************
    作者：张峻凡、荆煦添
    描述：生成被测物体脚本
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using Dummiesman;
using System.IO;

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
        if (model.ResourcePath.EndsWith("obj") || Path.HasExtension(model.ResourcePath)) CreateObj();
        else CreatePrefab();
    }
    private static void Create(GameObject obj)
    {
        ObjectValue objectValue = null;
        var model = RecordManager.tempRecord.showedObject;
        // 挂载组件
        // 遍历计算点
        Vector3 ClosestPoint = new Vector3();
        Vector3 FarthestPoint = new Vector3();
        int cnt = 0;
        foreach (Transform item in obj.transform)
        {
            // 挂载组件
            if ((objectValue = item.gameObject.GetComponent<ObjectValue>()) == null)
                (objectValue = item.gameObject.AddComponent<ObjectValue>()).ObjectModel = model;

            if (item.gameObject.GetComponent<MeshCollider>() == null)
            {
                var meshCollider = item.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.isTrigger = false;
            }
            //item.gameObject.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Objects/ImportObjectMaterial");//酸奶盒上色
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

            //item.gameObject.AddComponent<ImportModelSetColor>();//酸奶盒上色
            //item.gameObject.GetComponent<ImportModelSetColor>().SetColor();//
        }
        // 计算基础大小

        objectValue.BaseSize = new Vector3(Mathf.Abs(FarthestPoint.x - ClosestPoint.x),
            Mathf.Abs(FarthestPoint.y - ClosestPoint.y),
            Mathf.Abs(FarthestPoint.z - ClosestPoint.z));
        // 计算scale
        var max = Mathf.Max(objectValue.BaseSize.x, objectValue.BaseSize.y, objectValue.BaseSize.z);
        var scale = 2f / max;
        objectValue.BaseScale = scale;
        if (model.scale != 1)
            objectValue.Scale = model.scale;
        else
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
        foreach (Transform item in obj.transform)
        {
            item.localPosition = new Vector3(0, 0, 0);
        }
     }
    private static void CreateObj()
    {
        var model = RecordManager.tempRecord.showedObject;
        CommonTools.GetObject(model.ResourcePath, Create);
    }
    public static GameObject CreateSingleObj(string path)
    {
        GameObject gameObject = null;
        CommonTools.GetObject(path, x =>
        {
            gameObject = CreateSingleObj2(x);
        });
        return gameObject;
    }
    private static GameObject CreateSingleObj2(GameObject ret)
    {
        Vector3 ClosestPoint = new Vector3();
        Vector3 FarthestPoint = new Vector3();
        foreach (Transform item in ret.transform)
        {
            // 挂载组件
            item.gameObject.AddComponent<MeshCollider>();
            item.gameObject.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Objects/ImportObjectMaterial");
            item.gameObject.layer = 13;
            ResetModelPivot(item.gameObject);

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

            item.gameObject.AddComponent<ImportModelSetColor>();
            item.gameObject.GetComponent<ImportModelSetColor>().SetColor();

        }
        // 计算基础大小
        var BaseSize = new Vector3(Mathf.Abs(FarthestPoint.x - ClosestPoint.x),
            Mathf.Abs(FarthestPoint.y - ClosestPoint.y),
            Mathf.Abs(FarthestPoint.z - ClosestPoint.z));
        // 计算scale
        var max = Mathf.Max(BaseSize.x, BaseSize.y, BaseSize.z);
        var scale = 2f / max;
        ret.transform.localScale = new Vector3(scale, scale, scale);
        var rec = RecordManager.tempRecord;
        // 计算位置
        ret.transform.position = new Vector3(rec.objectStartPosition[0],
            rec.objectStartPosition[1] + BaseSize.y * scale / 2, rec.objectStartPosition[2]);

        return ret;
    }
    private static void CreatePrefab()
    {
        var model = RecordManager.tempRecord.showedObject;
        Main.m_Resource.LoadPrefab(new PrefabInfo(null, null, model.ResourcePath), null, loadDoneAction: x =>
            CreatePrefab2(x));
    }
    private static void CreatePrefab2(GameObject obj)
    {
        var model = RecordManager.tempRecord.showedObject;
        ObjectValue objectValue;
        // 挂载组件
        if ((objectValue = obj.GetComponent<ObjectValue>()) == null)
            (objectValue = obj.AddComponent<ObjectValue>()).ObjectModel = model;
        int cnt = 0;
        if (obj.GetComponent<Collider>() != null)
        {
            if (model.childrenPostition.Count == 0)
                model.childrenPostition.Add(obj.transform.localPosition.GetMyVector());
            else if (model.childrenPostition.Count > cnt)
                obj.transform.localPosition = model.childrenPostition[cnt];

            if (model.childrenRotation.Count == 0)
                model.childrenRotation.Add(obj.transform.localRotation.GetMyVector());
            else if (model.childrenRotation.Count > cnt)
                obj.transform.localRotation = model.childrenRotation[cnt];

            var right = obj.GetComponent<RightButtonObject>();
            if (right == null)
                right = obj.AddComponent<RightButtonObject>();
            right.objectValue = objectValue;
            right.index = 0;
            if (obj.GetComponent<Rigidbody>() == null)
            {
                var rigid = obj.AddComponent<Rigidbody>();
                rigid.useGravity = false;
                rigid.drag = 10;
                rigid.mass = 10;
                rigid.angularDrag = 10;
                rigid.isKinematic = false;
            }
        }
        // 遍历计算点
        Vector3 ClosestPoint = new Vector3();
        Vector3 FarthestPoint = new Vector3();

        foreach (var item in obj.GetComponentsInChildren<Collider>())
        {
            if (obj.GetComponent<Rigidbody>() == null)
            {
                var rigid = obj.AddComponent<Rigidbody>();
                rigid.useGravity = false;
                rigid.drag = 10;
                rigid.mass = 10;
                rigid.angularDrag = 10;
                rigid.isKinematic = false;
            }
            if (item.gameObject.GetComponent<RightButtonObject>() == null)
            {
                if (model.childrenPostition.Count == cnt)
                    model.childrenPostition.Add(item.transform.localPosition.GetMyVector());
                else if (model.childrenPostition.Count > cnt)
                    item.transform.localPosition = model.childrenPostition[cnt];

                if (model.childrenRotation.Count == cnt)
                    model.childrenRotation.Add(item.transform.localRotation.GetMyVector());
                else if (model.childrenRotation.Count > cnt)
                    item.transform.localRotation = model.childrenRotation[cnt];

                var right = item.gameObject.AddComponent<RightButtonObject>();
                right.objectValue = objectValue;
                right.index = cnt;
            }
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
        obj.SetActive(true);
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
