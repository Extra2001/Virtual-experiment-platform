/************************************************************************************
    作者：张柯
    描述：螺旋测微器行为处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
//注：由于unity中精度限制，游标卡尺的测量逻辑和千分尺完全不同
public class Micrometer_main : HTBehaviour
{
    private bool IsKaKaed=false;//

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
        if (this.transform.localPosition[1] >= 1.3156f)
        {
            IsKaKaed = true;
        }
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

    public void UsingO()
    {
        this.transform.Find("Micrometer_grandson").Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num = 3;
    }

    //通用行为
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
            Player_S.transform.rotation = Quaternion.Euler(new Vector3(0, 270f, 0));
            mCamera.transform.position = Ele.transform.position;
            mCamera.transform.rotation = Ele.transform.rotation;
            mCamera.fieldOfView = Ele.GetComponent<Camera>().fieldOfView;
            moveable_look = false;
            Nowin = true;
            Player_S.GetComponent<MirrorPlayer>().Nowin = Nowin;
            Player_S.GetComponent<MirrorPlayer>().moveable_look = moveable_look;
            Player_S.GetComponent<MirrorPlayer>().moveable_back = moveable_back;
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

        mCamera= GameObject.Find("FirstPersonCharacter").gameObject.GetComponent<Camera>();
        Ele = this.transform.Find("Micrometer_grandson").Find("014").Find("Camera").gameObject;
        Player_S = GameObject.Find("FPSController").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            UsingP();
        }
        if (Input.GetKey(KeyCode.O))
        {
            UsingO();
        }
        Look_back();

        if (this.transform.Find("Micrometer_grandson").Find("rotatebody_main").transform.localPosition[1] >= 1.3156f && this.transform.Find("Micrometer_grandson").Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num < 0)
        {
            this.transform.Find("Micrometer_grandson").Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num = 0;
        }
        if (this.transform.Find("Micrometer_grandson").Find("rotatebody_main").transform.localPosition[1] <=1.227f&& this.transform.Find("Micrometer_grandson").Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num > 0)
        {
            this.transform.Find("Micrometer_grandson").Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num = 0;
        }
    }
}
