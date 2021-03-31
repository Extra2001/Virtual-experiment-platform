using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
public class Camera_top : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject Cam2 = GameObject.Find("Camera_TOP");
        Cam2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
