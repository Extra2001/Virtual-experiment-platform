using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_place : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Enable = true;
    IndicatorManager indicatorManager;
    private bool moveable = false;
    public string name = "Vernier";
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
        if (Input.GetKey(KeyCode.B)&&Enable)
        {
            GetComponent<Back_place>().enabled = false;
            Manager.state.Value = 4;
            moveable =true;
        }
        if (moveable)
        {
            GameObject RuC = GameObject.Find("Camera");
            if (name == "Vernier")
            {
                RuC = GameObject.Find("Camera_onVernier");
            }
            if (name == "Ruler")
            {
                RuC = GameObject.Find("Camera");
            }
            this.transform.position += (RuC.transform.position - this.transform.position) / time;
            temp1 = this.transform.rotation.eulerAngles;
            temp2 = RuC.transform.rotation.eulerAngles;
            temp1 += (temp2 - temp1) / time;
            this.transform.rotation = Quaternion.Euler(temp1);
            mCamera.fieldOfView -= (mCamera.fieldOfView-19.0f)/ time;
        }
        if (mCamera.fieldOfView < 19.01f&&moveable)
        {
            GameObject RuC = GameObject.Find("Camera");
            if (name == "Vernier")
            {
                RuC = GameObject.Find("Camera_onVernier");
            }
            if (name == "Ruler")
            {
                RuC = GameObject.Find("Camera");
            }
            this.transform.position = RuC.transform.position;
            this.transform.rotation = RuC.transform.rotation;
            mCamera = gameObject.GetComponent<Camera>();
            mCamera.fieldOfView = 19.0f;
            moveable = false;
            GetComponent<Back_place>().enabled = true;
            indicatorManager = GameObject.Find("Indicator").GetComponent<IndicatorManager>();
            indicatorManager.Indicator1.ShowIndicate("V", "返回视角");
        }
    }


}
