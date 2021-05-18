using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look_ElectronicScalesSource : HTBehaviour
{
    //启用自动化
    public float time = 5.0f;
    public static bool Enable = true;
    private bool moveable = false;
    Camera mCamera;
    GameObject Player;
    GameObject Ele;
    public int FieldView;
    private Vector3 temp1 = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 temp2 = new Vector3(0.0f, 0.0f, 0.0f);
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mCamera = GameObject.Find("FirstPersonCharacter").gameObject.GetComponent<Camera>();
        Player = GameObject.Find("FirstPersonCharacter");
        Ele = GameObject.Find("FlagCamera_disable");
        if (Input.GetKey(KeyCode.F) && Enable)
        {
            GetComponent<Back_ElectronicScalesSource>().enabled = false;
            moveable = true;
        }
        if (moveable)
        {
            mCamera.transform.position += (Ele.transform.position - mCamera.transform.position) / time;
            temp1 = mCamera.transform.rotation.eulerAngles;
            temp2 = Ele.transform.rotation.eulerAngles;
            temp1 += (temp2 - temp1) / time;
            mCamera.transform.rotation = Quaternion.Euler(temp1);
            mCamera.fieldOfView -= (mCamera.fieldOfView - FieldView) / time;
        }
        if (mCamera.fieldOfView < FieldView+0.01 && moveable)
        {
            mCamera.transform.position = Ele.transform.position;
            mCamera.transform.rotation = Ele.transform.rotation;
            mCamera = gameObject.GetComponent<Camera>();
            mCamera.fieldOfView = FieldView;
            moveable = false;
            GetComponent<Back_ElectronicScalesSource>().enabled = true;
        }
    }
}
