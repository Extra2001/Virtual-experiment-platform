using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Micrometer_main : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    private bool IsKaKaed=false;//别骂了别骂了，这个名字搞笑的

    public AudioClip kakaka;//咔咔声

    private Vector3 PLC_eulerAngles;
    private Vector3 POS_eulerAngles;

    private Camera mCamera;
    private GameObject Ele;
    private GameObject Player_S;

    public Vector3 Ori_place;
    public Vector3 Ori_eulerAngles;
    public float Ori_fieldOfView;

    public float time = 15.0f;
    public bool moveable_look = false;
    public bool moveable_back = false;
    public bool Nowin=false;
    public void Measure()
    {
        if (IsKaKaed)
        {
            //播放螺旋测微计kaka的声音
            Main.m_Audio.PlaySingleSound(kakaka);
        }
        else
        {
            IsKaKaed = true;
        }
        this.transform.Find("Micrometer_grandson").Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num = -3;
    }

    private void Look_back()
    {
        if (Input.GetKey(KeyCode.X) )
        {
            if (Nowin == false)
            {

                //Player_S.GetComponent<FirstPersonController>().AbleCameraControl = false;
                Player_S.GetComponent<FirstPersonController>().m_WalkSpeed = 5;
               
                moveable_look = true;
                mCamera = GameObject.Find("FirstPersonCharacter").gameObject.GetComponent<Camera>();
                Ori_place = mCamera.transform.position;
                Ori_eulerAngles = mCamera.transform.rotation.eulerAngles;
                Ori_fieldOfView = mCamera.GetComponent<Camera>().fieldOfView;
            }
            else
            {
                moveable_back = true;
            }
            
        }
        Look();
        Back();
    }

    private void Look()
    {
        if (moveable_look)
        {
            mCamera.transform.position += (Ele.transform.position - mCamera.transform.position) / time;
            PLC_eulerAngles = mCamera.transform.rotation.eulerAngles;
            POS_eulerAngles = Ele.transform.rotation.eulerAngles;
            PLC_eulerAngles += (POS_eulerAngles - PLC_eulerAngles) / time;
            mCamera.transform.rotation = Quaternion.Euler(PLC_eulerAngles);
            mCamera.fieldOfView -= (mCamera.fieldOfView - Ele.GetComponent<Camera>().fieldOfView) / time;
        }
        if (mCamera.fieldOfView < Ele.GetComponent<Camera>().fieldOfView + 0.01 && moveable_look)
        {
            mCamera.transform.position = Ele.transform.position;
            mCamera.transform.rotation = Ele.transform.rotation;
            mCamera.fieldOfView = Ele.GetComponent<Camera>().fieldOfView;
            moveable_look = false;
            Nowin = true;
        }
    }
    private void Back()
    {
        if (moveable_back)
        {
            mCamera.transform.position += (Ori_place - mCamera.transform.position) / time;
            PLC_eulerAngles = mCamera.transform.rotation.eulerAngles;
            POS_eulerAngles = Ori_eulerAngles;
            PLC_eulerAngles += (POS_eulerAngles - PLC_eulerAngles) / time;
            mCamera.transform.rotation = Quaternion.Euler(PLC_eulerAngles);
            mCamera.fieldOfView += (Ori_fieldOfView - mCamera.fieldOfView) / time;
        }
        if (mCamera.fieldOfView > Ori_fieldOfView - 0.01 && moveable_back)
        {
            mCamera.transform.position = Ori_place;
            mCamera.transform.rotation = Quaternion.Euler(Ori_eulerAngles); 
            mCamera.fieldOfView = Ori_fieldOfView;
            moveable_back = false;
            Nowin = false;
            //Player_S.GetComponent<FirstPersonController>().AbleCameraControl = true;
           // Player_S.GetComponent<FirstPersonController>().WalkSpeed = 30;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        mCamera= GameObject.Find("FirstPersonCharacter").gameObject.GetComponent<Camera>();
        Ele = this.transform.Find("Micrometer_grandson").Find("014").Find("Camera").gameObject;
        Player_S = GameObject.Find("FPSController").gameObject;
        //this.transform.Find("Micrometer_grandson").Find("rotatebody_main").Find("srick").transform.localPosition -= new Vector3(0, (0.53f * ZeroPointError) / 5000f, 0);
    }

    // Update is called once per frame
    void Update()
    {

        Look_back();
    }
}
