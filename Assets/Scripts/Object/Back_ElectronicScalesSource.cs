using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_ElectronicScalesSource : HTBehaviour
{
    //启用自动化
    public float time = 5.0f;
    public static bool Enable = true;
    private bool moveable = false;
    Camera mCamera;
    GameObject Player;
    GameObject Ele;
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
        if (Input.GetKey(KeyCode.V) && Enable)
        {
            moveable = true;
            GetComponent<Look_ElectronicScalesSource>().enabled = false;
        }
        if (moveable)
        {
            mCamera.transform.position += (Ele.transform.position - mCamera.transform.position) / time;
            temp1 = mCamera.transform.rotation.eulerAngles;
            temp2 = Ele.transform.rotation.eulerAngles;
            temp1 += (temp2 - temp1) / time;
            mCamera.transform.rotation = Quaternion.Euler(temp1);
            mCamera.fieldOfView -= (mCamera.fieldOfView - 60.0f) / time;
        }
        if (mCamera.fieldOfView > 59.99f && moveable)
        {
            mCamera.transform.position = Ele.transform.position;
            mCamera.transform.rotation = Ele.transform.rotation;
            mCamera.fieldOfView = 60.0f;
            moveable = false;
            GetComponent<Look_ElectronicScalesSource>().enabled = true;
        }
    }
}
