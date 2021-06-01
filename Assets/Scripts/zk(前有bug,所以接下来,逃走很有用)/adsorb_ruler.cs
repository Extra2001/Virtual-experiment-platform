using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adsorb_ruler : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public float t = 15.0f;
    public bool Doing = true;


    private GameObject[] m_Desk;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //if (!Input.GetMouseButton(1))
        //{
        //    if (!Input.GetMouseButton(0))
        //    {
        //        for (int i = 0; i < m_Desk.Length; i++)
        //        {
        //            GameObject RU = m_Desk[i];
        //            if (Vector3.Distance(RU.transform.position, BO.transform.position) <= 0.3f && Doing)
        //            {
        //                transform.position += (BO.transform.position - RU.transform.position) / t;
        //            }
        //            else if (Vector3.Distance(RU.transform.position, BO.transform.position) > 0.3f)
        //            {
        //                Doing = true;
        //            }
        //        }
        //    }
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Measured")
        {
            Doing = false;
        }

    }
    private void absord()
    {
            GameObject RU = GameObject.Find("ruler");
            GameObject BO = GameObject.Find("book_0001b");
            transform.position += (BO.transform.position - RU.transform.position)/t;
    }
}
