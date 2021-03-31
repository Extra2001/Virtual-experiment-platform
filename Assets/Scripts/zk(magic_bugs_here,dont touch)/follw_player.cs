using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follw_player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject pl = GameObject.Find("MainCamera");
        this.transform.position = pl.transform.position;
    }
}
