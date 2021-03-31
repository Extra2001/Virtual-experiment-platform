using RTEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Come_for : MonoBehaviour
{
    private Vector3 PLC=new Vector3( 1.18f,2.0f,-1.38f);
    private Vector3 PL = new Vector3(1.18f, 1.6f, -1.58f);
    private Vector3 CPC = new Vector3(0, 0, 0.304f);
    public bool moveable = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject CP = GameObject.Find("Camera_ruler");
        print(transform.TransformPoint(CP.transform.position));
        print(transform.TransformPoint(CP.transform.localPosition));
        print(CP.transform.position);
        print(CP.transform.localPosition);
    }
    private void Waggggggggg()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject CP = GameObject.Find("Camera_ruler");
        GameObject RU = GameObject.Find("ruler");
        GameObject  PLayer= GameObject.Find("Player");
        if (Input.GetKey(KeyCode.Z))
        {
            transform.localPosition = RU.transform.localPosition+CP.transform.localPosition- PLayer.transform.localPosition;
            moveable = true;
        }
        if (moveable)
        {
       
            //GameObject CP = GameObject.Find("Camera_ruler");
            //Vector3 CPL = CP.transform.localPosition;
            //if (Vector3.Distance(transform.TransformPoint(CP.transform.localPosition), transform.TransformPoint(this.transform.localPosition)) >= 0.01f)
            //{
            //    moveable = false;
            //    this.transform.localPosition = transform.TransformPoint(CP.transform.localPosition) - PL;
            //}
            //else
            //{
            //    this.transform.localPosition += transform.TransformPoint(CP.transform.localPosition) - transform.TransformPoint(this.transform.localPosition) /5.0f;
            //}
        }

    }
}
