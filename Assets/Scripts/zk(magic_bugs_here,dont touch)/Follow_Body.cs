using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Body : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject B = GameObject.Find("MeasureBody");
        B.transform.position = this.transform.position;
        B.transform.rotation = this.transform.rotation;
    }
}
