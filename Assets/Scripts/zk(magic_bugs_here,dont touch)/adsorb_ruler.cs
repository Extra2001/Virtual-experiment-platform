using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adsorb_ruler : MonoBehaviour
{
    public float t = 15.0f;
    public bool Doing = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        GameObject RU = GameObject.Find("ruler");
        GameObject BO = GameObject.Find("book_0001b");
        if (!Input.GetMouseButton(1))
        {
            if (!Input.GetMouseButton(0))
            {
                if (Vector3.Distance(RU.transform.position, BO.transform.position) <= 0.3f && Doing)
                {
                    absord();
                }
                else if(Vector3.Distance(RU.transform.position, BO.transform.position) > 0.3f)
                {
                    Doing = true;
                }

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "book")
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
