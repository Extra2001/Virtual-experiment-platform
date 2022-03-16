/************************************************************************************
    作者：张柯
    描述：游标卡尺行为处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using DG.Tweening;
public class Caliper_main : HTBehaviour
{
    //启用自动化
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
    //定义游标卡尺行为
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

    public void UsingP()
    {
        this.transform.Find("MeasureHead").gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -0.3f), ForceMode.VelocityChange);
    }

    public void UsingO()
    {
        this.transform.Find("MeasureHead").gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 0.2f), ForceMode.VelocityChange);
    }

    public void ini()
    {
        mCamera = GameObject.Find("FirstPersonCharacter").gameObject.GetComponent<Camera>();
        Ele = this.transform.Find("MeasureHead").Find("Camera").gameObject;
        Player_S = GameObject.Find("FPSController").gameObject;
    }

    public void Measure()
    {
        this.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num = -0.1f;
    }
    public void BackMeasure()
    {
        this.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num = 0.1f;
    }

    private void Look_back()
    {
        //控制逻辑，按X键进行缩放/复原，调用FPScontrol中存放的之前位置
        //已修正
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
            DOTween.To(()=>mCamera.fieldOfView, x=> mCamera.fieldOfView = x, Ele.GetComponent<Camera>().fieldOfView, 0.8f)
                .SetEase(Ease.OutExpo);
            moveable_look = false;
            Nowin = true;
            Player_S.GetComponent<MirrorPlayer>().Nowin = Nowin;
            Player_S.GetComponent<MirrorPlayer>().moveable_look = moveable_look;
            Player_S.GetComponent<MirrorPlayer>().moveable_back = moveable_back;
        }
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
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ini();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("p");
            UsingP();
        }
        if (Input.GetKey(KeyCode.O))
        {
            Debug.Log("o");
            UsingO();
        }
        Look_back();
        //判断游标卡尺是否到两侧顶点
        if (this.transform.Find("MeasureHead").gameObject.transform.localPosition[0] >=0&& this.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num<0)
        { 
            this.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num = 0;
        }
        if (this.transform.Find("MeasureHead").gameObject.transform.localPosition[0] <= -0.45f&& this.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num > 0)
        {
            this.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num = 0;
        }
    }
}
