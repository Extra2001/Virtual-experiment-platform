/************************************************************************************
    作者：张柯
    描述：电流表行为处理程序
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class RotateAmmeter : HTBehaviour
{
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
    public bool Nowin = false;
    //启用自动化
    public float MaxA = 3.0f;
    public float NowA=0.0f;
    private float TarA;
    private bool OnGoing = false;
    public float times = 15.0f;
    public float ii;

    protected override bool IsAutomate => true;


    public void UsingX()
    {
        Nowin = Player_S.GetComponent<MirrorPlayer>().Nowin;
        moveable_look = Player_S.GetComponent<MirrorPlayer>().moveable_look;
        moveable_back = Player_S.GetComponent<MirrorPlayer>().moveable_back;
        if (!Nowin && !moveable_look)
        {

            Player_S.GetComponent<FirstPersonController>().m_MouseLookRotate = false;
            Player_S.GetComponent<FirstPersonController>().m_WalkSpeed = 1;


            moveable_look = true;
            Player_S.GetComponent<MirrorPlayer>().moveable_look = moveable_look;
            Player_S.GetComponent<MirrorPlayer>().updateMirror();
        }
        else if (Nowin)
        {
            moveable_back = true;
            Player_S.GetComponent<MirrorPlayer>().moveable_back = moveable_back;
        }
    }

    public void ini()
    {
        mCamera = GameObject.Find("FirstPersonCharacter").gameObject.GetComponent<Camera>();
        Ele = this.transform.Find("Camera").gameObject;
        Player_S = GameObject.Find("FPSController").gameObject;
    }

    public void ShowNum(float num)
    {
        //插值导入旋转动画，详见内部实现
        transform.Find("Cylinder005").transform.DOLocalRotate(new Vector3(0, 207.5f-82.5f*num /MaxA, 0), 1f).SetEase(Ease.OutExpo);
    }
    //通用缩放视角函数
    private void Look_back()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            UsingX();
        }
        Ori_place = Player_S.GetComponent<MirrorPlayer>().Ori_place;
        Ori_eulerAngles = Player_S.GetComponent<MirrorPlayer>().Ori_eulerAngles;
        Ori_fieldOfView = Player_S.GetComponent<MirrorPlayer>().Ori_fieldOfView;
        Look();
        Back();
    }

    //通用，视角拉近行为
    private void Look()
    {
        if (moveable_look)
        {
            mCamera.transform.DOMove(Ele.transform.position, 0.8f)
                .SetEase(Ease.OutExpo);
            mCamera.transform.DORotate(Ele.transform.rotation.eulerAngles, 0.8f)
                .SetEase(Ease.OutExpo);
            DOTween.To(() => mCamera.fieldOfView, x => mCamera.fieldOfView = x, Ele.GetComponent<Camera>().fieldOfView, 0.8f)
                .SetEase(Ease.OutExpo);
            moveable_look = false;
            Nowin = true;
            Player_S.GetComponent<MirrorPlayer>().Nowin = Nowin;
            Player_S.GetComponent<MirrorPlayer>().moveable_look = moveable_look;
            Player_S.GetComponent<MirrorPlayer>().moveable_back = moveable_back;
            //mCamera.transform.position += (Ele.transform.position - mCamera.transform.position) / time;
            //PLC_eulerAngles = mCamera.transform.rotation.eulerAngles;
            //POS_eulerAngles = Ele.transform.rotation.eulerAngles;
            //PLC_eulerAngles += (POS_eulerAngles - PLC_eulerAngles) / time;
            //mCamera.transform.rotation = Quaternion.Euler(PLC_eulerAngles);
            //mCamera.fieldOfView -= (mCamera.fieldOfView - Ele.GetComponent<Camera>().fieldOfView) / time;
        }
        //if (mCamera.fieldOfView < Ele.GetComponent<Camera>().fieldOfView + 0.01 && moveable_look)
        //{
        //    Player_S.transform.rotation = Quaternion.Euler(new Vector3(0, 270f, 0));
        //    mCamera.transform.position = Ele.transform.position;
        //    mCamera.transform.rotation = Ele.transform.rotation;
        //    mCamera.fieldOfView = Ele.GetComponent<Camera>().fieldOfView;
        //    moveable_look = false;
        //    Nowin = true;
        //    Player_S.GetComponent<MirrorPlayer>().Nowin = Nowin;
        //    Player_S.GetComponent<MirrorPlayer>().moveable_look = moveable_look;
        //    Player_S.GetComponent<MirrorPlayer>().moveable_back = moveable_back;
        //}
    }
    //通用，视角拉回行为
    private void Back()
    {
        if (moveable_back)
        {
            mCamera.transform.DOMove(Ori_place, 0.8f)
                .SetEase(Ease.OutExpo);
            mCamera.transform.DORotate(Ori_eulerAngles, 0.8f)
                .SetEase(Ease.OutExpo);
            DOTween.To(() => mCamera.fieldOfView, x => mCamera.fieldOfView = x, Ori_fieldOfView, 0.8f)
                .SetEase(Ease.OutExpo);
            mCamera.transform.position += (Ori_place - mCamera.transform.position) / time;
            moveable_back = false;
            Nowin = false;
            Player_S.GetComponent<FirstPersonController>().m_MouseLookRotate = true;
            Player_S.GetComponent<FirstPersonController>().m_WalkSpeed = 30;
            Player_S.GetComponent<MirrorPlayer>().Nowin = Nowin;
            Player_S.GetComponent<MirrorPlayer>().moveable_look = moveable_look;
            Player_S.GetComponent<MirrorPlayer>().moveable_back = moveable_back;
            //PLC_eulerAngles = mCamera.transform.rotation.eulerAngles;
            //POS_eulerAngles = Ori_eulerAngles;
            //PLC_eulerAngles += (POS_eulerAngles - PLC_eulerAngles) / time;
            //mCamera.transform.rotation = Quaternion.Euler(PLC_eulerAngles);
            //mCamera.fieldOfView += (Ori_fieldOfView - mCamera.fieldOfView) / time;
        }
        //if (mCamera.fieldOfView > Ori_fieldOfView - 0.01 && moveable_back)
        //{
        //    mCamera.transform.position = Ori_place;
        //    mCamera.transform.rotation = Quaternion.Euler(Ori_eulerAngles);
        //    //Player_S.transform.rotation = mCamera.transform.rotation;
        //    mCamera.fieldOfView = Ori_fieldOfView;
        //    moveable_back = false;
        //    Nowin = false;
        //    Player_S.GetComponent<FirstPersonController>().m_MouseLookRotate = true;
        //    Player_S.GetComponent<FirstPersonController>().m_WalkSpeed = 30;
        //    Player_S.GetComponent<MirrorPlayer>().Nowin = Nowin;
        //    Player_S.GetComponent<MirrorPlayer>().moveable_look = moveable_look;
        //    Player_S.GetComponent<MirrorPlayer>().moveable_back = moveable_back;

        //}
    }


    void Start()
    {
        ini();
    }

    private void Update()
    {
        Look_back();
    }

}
