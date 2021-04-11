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
    private ObjectsModel ShowedObject=null;

    public static GameObject MyObject;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(CreateOrDestory);
    }

    private void CreateOrDestory()
    {
        if (ShowedObject != null)
        {

        }

        MyObject = Main.m_ObjectPool.Spawn(objects.id.ToString());
        //初始化被测物体设置
        MyObject.AddComponent<MeshCollider>();
        MyObject.GetComponent<MeshCollider>().convex = true;
        MyObject.GetComponent<MeshCollider>().isTrigger = true;
        MyObject.AddComponent<Rigidbody>();
        MyObject.GetComponent<Rigidbody>().useGravity = false;
        MyObject.GetComponent<Rigidbody>().isKinematic = true;

        Main.m_UI.CloseUI<BagControl>();
    }
}
