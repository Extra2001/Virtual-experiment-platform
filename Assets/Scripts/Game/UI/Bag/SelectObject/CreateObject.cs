using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateObject : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public ObjectsModel objects = null;
    private ObjectsModel ShowedObject = null;

    private GameObject MyObject;
    private float Length;
    private float Width;
    private float Height;
    private float Mass;//估计值
    private float Density = 1;//密度，瞎定的一个值


    private Vector3 ClosestPoint = new Vector3();//最近点
    private Vector3 FarthestPoint = new Vector3();//最远点
    private Vector3 Temp = new Vector3();
    private float MaxDistence = 2f; //物体大小的最大限度
    private float MinDistence = 0.01f; //物体大小的最小限度
    private float TempRate;



    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(CreateWithDestory);
    }

    public static void CreateWithoutDestory()
    {
        //变量复用

        GameObject MyObject;
        float Length;
        float Width;
        float Height;
        float Mass;//估计值
        float Density = 1;//密度，瞎定的一个值


        Vector3 ClosestPoint = new Vector3();//最近点
        Vector3 FarthestPoint = new Vector3();//最远点
        Vector3 Temp = new Vector3();
        float MaxDistence = 2f; //物体大小的最大限度
        float MinDistence = 0.01f; //物体大小的最小限度

        ObjectsModel ShowedObject = RecordManager.tempRecord.ShowedObject;

        //

        MyObject = Main.m_ObjectPool.Spawn(ShowedObject.id.ToString());
        if (MyObject.GetComponent<ObjectValue>() == null)
        {
            MyObject.AddComponent<ObjectValue>();
        }
        
        GameManager.Instance.MyObject = MyObject;


        //初始化被测物体设置
        List<Transform> GameObjectList = new List<Transform>();
        foreach (Transform child in MyObject.transform)
        {
            GameObjectList.Add(child);
        }
        for (int i = 0; i < GameObjectList.Count; i++)
        {
            //加组件
            GameObjectList[i].gameObject.tag = ("Tools_Be_Moved");
            GameObjectList[i].gameObject.layer = 11;                    //layer的序号
            if (GameObjectList[i].gameObject.GetComponent<MeshCollider>() == null)
            {
                GameObjectList[i].gameObject.AddComponent<MeshCollider>();
                GameObjectList[i].gameObject.GetComponent<MeshCollider>().convex = true;
                GameObjectList[i].gameObject.GetComponent<MeshCollider>().isTrigger = true;
            }
            if (GameObjectList[i].gameObject.GetComponent<Rigidbody>() == null)
            {
                GameObjectList[i].gameObject.AddComponent<Rigidbody>();
                GameObjectList[i].gameObject.GetComponent<Rigidbody>().useGravity = false;
                GameObjectList[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
           
            //获取所有子物体中的最近点和最远点
            Temp = GameObjectList[i].gameObject.GetComponent<MeshFilter>().mesh.bounds.min;
            if (Temp.x < ClosestPoint.x)
            {
                ClosestPoint.x = Temp.x;
            }
            if (Temp.y < ClosestPoint.y)
            {
                ClosestPoint.y = Temp.y;
            }
            if (Temp.z < ClosestPoint.z)
            {
                ClosestPoint.z = Temp.z;
            }
            Temp = GameObjectList[i].gameObject.GetComponent<MeshFilter>().mesh.bounds.max;
            if (Temp.x > FarthestPoint.x)
            {
                FarthestPoint.x = Temp.x;
            }
            if (Temp.y > FarthestPoint.y)
            {
                FarthestPoint.y = Temp.y;
            }
            if (Temp.z > FarthestPoint.z)
            {
                FarthestPoint.z = Temp.z;
            }

        }
        Length = Mathf.Abs(FarthestPoint.x - ClosestPoint.x);
        Width = Mathf.Abs(FarthestPoint.y - ClosestPoint.y);
        Height = Mathf.Abs(FarthestPoint.z - ClosestPoint.z);

        //防止被错误缩放
        while (RecordManager.tempRecord.ShowedObject.Rate > 1.1f)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x / 10f, MyObject.transform.localScale.y / 10f, MyObject.transform.localScale.z / 10f);
            Debug.Log("缩小0");
            //Length *= 10f;
            //Width *= 10f;
            //Height *= 10f;
            RecordManager.tempRecord.ShowedObject.Rate /= 10f;
        }
        while (RecordManager.tempRecord.ShowedObject.Rate < 0.9f)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x * 10f, MyObject.transform.localScale.y * 10f, MyObject.transform.localScale.z * 10f);
            Debug.Log("RecordManager.tempRecord.Rate:" + RecordManager.tempRecord.ShowedObject.Rate + "放大");
            //Length /= 10f;
            //Width /= 10f;
            //Height /= 10f;
            RecordManager.tempRecord.ShowedObject.Rate *= 10f;
        }




        //将物体缩放至合适大小
        while (Length > MaxDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x / 10f, MyObject.transform.localScale.y / 10f, MyObject.transform.localScale.z / 10f);
            Length /= 10f;
            Width /= 10f;
            Height /= 10f;
            RecordManager.tempRecord.ShowedObject.Rate /= 10f;
        }
        while (Length < MinDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x * 10f, MyObject.transform.localScale.y * 10f, MyObject.transform.localScale.z * 10f);
            Length *= 10f;
            Width *= 10f;
            Height *= 10f;
            RecordManager.tempRecord.ShowedObject.Rate *= 10f;
        }
        while (Width > MaxDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x / 10f, MyObject.transform.localScale.y / 10f, MyObject.transform.localScale.z / 10f);
            Length /= 10f;
            Width /= 10f;
            Height /= 10f;
            RecordManager.tempRecord.ShowedObject.Rate /= 10f;
        }
        while (Width < MinDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x * 10f, MyObject.transform.localScale.y * 10f, MyObject.transform.localScale.z * 10f);
            Length *= 10f;
            Width *= 10f;
            Height *= 10f;
            RecordManager.tempRecord.ShowedObject.Rate *= 10f;
        }
        while (Height > MaxDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x / 10f, MyObject.transform.localScale.y / 10f, MyObject.transform.localScale.z / 10f);
            Length /= 10f;
            Width /= 10f;
            Height /= 10f;
            RecordManager.tempRecord.ShowedObject.Rate /= 10f;
        }
        while (Height < MinDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x * 10f, MyObject.transform.localScale.y * 10f, MyObject.transform.localScale.z * 10f);
            Length *= 10f;
            Width *= 10f;
            Height *= 10f;
            RecordManager.tempRecord.ShowedObject.Rate *= 10f;
        }

        //移到合适位置
        MyObject.transform.position = new Vector3(RecordManager.tempRecord.ObjectStartPosition[0], RecordManager.tempRecord.ObjectStartPosition[1] + Width, RecordManager.tempRecord.ObjectStartPosition[2]);

        //记录赋值


        Mass = Density * Length * Width * Height;
        MyObject.GetComponent<ObjectValue>().Length = Length;
        MyObject.GetComponent<ObjectValue>().Width = Width;
        MyObject.GetComponent<ObjectValue>().Height = Height;
        MyObject.GetComponent<ObjectValue>().Mass = Mass;

    }

    private void CreateWithDestory()
    {
        ShowedObject = RecordManager.tempRecord.ShowedObject;

        if (ShowedObject != null)
        {
            TempRate = ShowedObject.Rate;
            Main.m_ObjectPool.Despawn(ShowedObject.id.ToString(), GameManager.Instance.MyObject);
        }
        RecordManager.tempRecord.ShowedObject = objects;
        RecordManager.tempRecord.ShowedObject.Rate = TempRate;
        MyObject = Main.m_ObjectPool.Spawn(objects.id.ToString());
        if (MyObject.GetComponent<ObjectValue>() == null)
        {
            MyObject.AddComponent<ObjectValue>();
        }
        
        GameManager.Instance.MyObject = MyObject;

        //初始化被测物体设置
        List<Transform> GameObjectList = new List<Transform>();
        foreach (Transform child in MyObject.transform)
        {
            GameObjectList.Add(child);
        }
        for (int i = 0; i < GameObjectList.Count; i++)
        {
            //加组件
            GameObjectList[i].gameObject.tag = ("Tools_Be_Moved");
            GameObjectList[i].gameObject.layer = 11;                    //layer的序号
            if (GameObjectList[i].gameObject.GetComponent<MeshCollider>() == null)
            {
                GameObjectList[i].gameObject.AddComponent<MeshCollider>();
                GameObjectList[i].gameObject.GetComponent<MeshCollider>().convex = true;
                GameObjectList[i].gameObject.GetComponent<MeshCollider>().isTrigger = true;
            }
            if (GameObjectList[i].gameObject.GetComponent<Rigidbody>() == null)
            {
                GameObjectList[i].gameObject.AddComponent<Rigidbody>();
                GameObjectList[i].gameObject.GetComponent<Rigidbody>().useGravity = false;
                GameObjectList[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            //获取所有子物体中的最近点和最远点
            Temp = GameObjectList[i].gameObject.GetComponent<MeshFilter>().mesh.bounds.min;
            if (Temp.x < ClosestPoint.x)
            {
                ClosestPoint.x = Temp.x;
            }
            if (Temp.y < ClosestPoint.y)
            {
                ClosestPoint.y = Temp.y;
            }
            if (Temp.z < ClosestPoint.z)
            {
                ClosestPoint.z = Temp.z;
            }
            Temp = GameObjectList[i].gameObject.GetComponent<MeshFilter>().mesh.bounds.max;
            if (Temp.x > FarthestPoint.x)
            {
                FarthestPoint.x = Temp.x;
            }
            if (Temp.y > FarthestPoint.y)
            {
                FarthestPoint.y = Temp.y;
            }
            if (Temp.z > FarthestPoint.z)
            {
                FarthestPoint.z = Temp.z;
            }

        }
        Length = Mathf.Abs(FarthestPoint.x - ClosestPoint.x);
        Width = Mathf.Abs(FarthestPoint.y - ClosestPoint.y);
        Height = Mathf.Abs(FarthestPoint.z - ClosestPoint.z);

        while (RecordManager.tempRecord.ShowedObject.Rate > 1.1f)
        {
            Debug.Log("RecordManager.tempRecord.Rate:" + RecordManager.tempRecord.ShowedObject.Rate);
            Debug.Log("缩小");
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x / 10f, MyObject.transform.localScale.y / 10f, MyObject.transform.localScale.z / 10f);
            //Length *= 10f;
            //Width *= 10f;
            //Height *= 10f;
            RecordManager.tempRecord.ShowedObject.Rate /= 10f;
        }
        while (RecordManager.tempRecord.ShowedObject.Rate < 0.9f)
        {
            Debug.Log("RecordManager.tempRecord.Rate:" + RecordManager.tempRecord.ShowedObject.Rate + "放大");
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x * 10f, MyObject.transform.localScale.y * 10f, MyObject.transform.localScale.z * 10f);
            //Length /= 10f;
            //Width /= 10f;
            //Height /= 10f;
            RecordManager.tempRecord.ShowedObject.Rate *= 10f;
        }

        Debug.Log("Length"+Length);
        Debug.Log("Width" + Width);
        Debug.Log("Height" + Height);


        //将物体缩放至合适大小
        while (Length > MaxDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x / 10f, MyObject.transform.localScale.y / 10f, MyObject.transform.localScale.z / 10f);
            Length /= 10f;
            Width /= 10f;
            Height /= 10f;
            RecordManager.tempRecord.ShowedObject.Rate /= 10f;
        }
        while (Length < MinDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x * 10f, MyObject.transform.localScale.y * 10f, MyObject.transform.localScale.z * 10f);
            Length *= 10f;
            Width *= 10f;
            Height *= 10f;
            RecordManager.tempRecord.ShowedObject.Rate *= 10f;
        }
        while (Width > MaxDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x / 10f, MyObject.transform.localScale.y / 10f, MyObject.transform.localScale.z / 10f);
            Length /= 10f;
            Width /= 10f;
            Height /= 10f;
            RecordManager.tempRecord.ShowedObject.Rate /= 10f;
        }
        while (Width < MinDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x * 10f, MyObject.transform.localScale.y * 10f, MyObject.transform.localScale.z * 10f);
            Length *= 10f;
            Width *= 10f;
            Height *= 10f;
            RecordManager.tempRecord.ShowedObject.Rate *= 10f;
        }
        while (Height > MaxDistence)
        {
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x / 10f, MyObject.transform.localScale.y / 10f, MyObject.transform.localScale.z / 10f);
            Length /= 10f;
            Width /= 10f;
            Height /= 10f;
            RecordManager.tempRecord.ShowedObject.Rate /= 10f;
        }
        while (Height < MinDistence)
        {            
            MyObject.transform.localScale = new Vector3(MyObject.transform.localScale.x * 10f, MyObject.transform.localScale.y * 10f, MyObject.transform.localScale.z * 10f);
            Length *= 10f;
            Width *= 10f;
            Height *= 10f;
            RecordManager.tempRecord.ShowedObject.Rate *= 10f;
        }

        //移到合适位置
        MyObject.transform.position = new Vector3(RecordManager.tempRecord.ObjectStartPosition[0], RecordManager.tempRecord.ObjectStartPosition[1] + Width, RecordManager.tempRecord.ObjectStartPosition[2]);

        //记录赋值


        Mass = Density * Length * Width * Height;
        MyObject.GetComponent<ObjectValue>().Length = Length;
        MyObject.GetComponent<ObjectValue>().Width = Width;
        MyObject.GetComponent<ObjectValue>().Height = Height;
        MyObject.GetComponent<ObjectValue>().Mass = Mass;


        Main.m_UI.CloseUI<BagControl>();
    }

    private void Update()
    {
        Debug.Log(RecordManager.tempRecord.ShowedObject.Rate);
    }
}
