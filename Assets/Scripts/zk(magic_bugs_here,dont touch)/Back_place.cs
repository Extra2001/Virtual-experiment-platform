using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_place : MonoBehaviour
{
    public bool Enable = true;
    private bool moveable = false;
    IndicatorManager indicatorManager;
    private Vector3 temp1 = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 temp2 = new Vector3(0.0f, 0.0f, 0.0f);
    public float time = 5.0f;
    Camera mCamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mCamera = gameObject.GetComponent<Camera>();
        if (Input.GetKey(KeyCode.V) && Enable)
        {
            moveable = true;
            GetComponent<Show_place>().enabled = false;
        }
        if (moveable)
        {
            GameObject RuC = GameObject.Find("Camera_invisiable");
            this.transform.position += (RuC.transform.position - this.transform.position) / time;
            temp1 = this.transform.rotation.eulerAngles;
            temp2 = RuC.transform.rotation.eulerAngles;
            temp1 += (temp2 - temp1) / time;
            this.transform.rotation = Quaternion.Euler(temp1);
            mCamera.fieldOfView -= (mCamera.fieldOfView - 60.0f) / time;
        }
        if (mCamera.fieldOfView > 59.99f && moveable)
        {
            GameObject RuC = GameObject.Find("Camera_invisiable");
            this.transform.position = RuC.transform.position;
            this.transform.rotation = RuC.transform.rotation;
            mCamera = gameObject.GetComponent<Camera>();
            mCamera.fieldOfView = 60.0f;
            moveable = false;
            GetComponent<Show_place>().enabled = true;
            indicatorManager = GameObject.Find("Indicator").GetComponent<IndicatorManager>();
            indicatorManager.Indicator1.ShowIndicate("B", "贴近测量");
        }
    }
}

