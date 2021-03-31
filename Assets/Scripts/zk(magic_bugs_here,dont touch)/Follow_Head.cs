using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Head : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject H = GameObject.Find("MeasureHead");
        H.transform.position = this.transform.position + new Vector3(0.0f, 0.0f, 0.2295331f);
        H.transform.rotation = this.transform.rotation;
    }
}
