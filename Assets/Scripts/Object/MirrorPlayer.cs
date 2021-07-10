/************************************************************************************
    作者：张柯
    描述：切换测量视角处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;

public class MirrorPlayer : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public Vector3 Ori_place;
    public Vector3 Ori_eulerAngles;
    public float Ori_fieldOfView;
    private Camera mCamera;
    public bool moveable_look = false;
    public bool moveable_back = false;
    public bool Nowin = false;

    public void updateMirror()
    {
        mCamera = GameObject.Find("FirstPersonCharacter").gameObject.GetComponent<Camera>();
        Ori_place = mCamera.transform.position;
        Ori_eulerAngles = mCamera.transform.rotation.eulerAngles;
        Ori_fieldOfView = mCamera.GetComponent<Camera>().fieldOfView;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
