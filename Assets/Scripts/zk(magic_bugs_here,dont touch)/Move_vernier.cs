using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Move_vernier : MonoBehaviour
{
    private float distance = 0.5f;
    public bool move1 = true;
    public bool move2 = true;
    private new Vector3 del;
    private Vector3 Temp = new Vector3(0.0f, 0.0f, 0.0f); 
    // Start is called before the first frame update

    void Start()
    {
        
    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.O))
        {
            Code();
        }
        if (Input.GetKey(KeyCode.P))
        {
            Code2();
        }
    }
    void Code()
    {
        
        GameObject V1 = GameObject.Find("Real_Vernier");
        GameObject V2 = GameObject.Find("MeasureHead");
        GameObject V3 = GameObject.Find("book_0001b");
        V1.GetComponent<V1_move>().doing=true;
        V2.GetComponent<V2_move>().doing = true;
        if (move1 && move2)
        {
            V1.transform.position += 0.0022f * (V2.transform.position - V1.transform.position).normalized;
            V2.transform.position -= 0.0044f * (V2.transform.position - V1.transform.position).normalized;
        }
        else if (move1 && !move2)
        {
            Temp = V2.transform.position;
            V1.transform.position += 0.0022f * (V2.transform.position - V1.transform.position).normalized;
            V2.transform.position = Temp;
        }
        else if (!move1 && move2)
        {
            V2.transform.position -= 0.0022f * (V2.transform.position - V1.transform.position).normalized;
        }
        if ((V1.transform.position - V2.transform.position).magnitude <= 0.003f&&move1&&move2)
        {
            V1.transform.position = 0.4999f * (V1.transform.position - V2.transform.position) + V2.transform.position;
            V2.transform.position = 0.4999f*(V2.transform.position - V1.transform.position)+V1.transform.position;
            //V1.transform.position = V2.transform.position;
            print((V1.transform.position - V2.transform.position).magnitude);
            move1 = false;
            move2 = false;
        }

    }

    void Code2()
    {

        GameObject V1 = GameObject.Find("Real_Vernier");
        GameObject V2 = GameObject.Find("MeasureHead");
        V1.GetComponent<V1_move>().doing = false;
        V2.GetComponent<V2_move>().doing = false;
        if ((V1.transform.position - V2.transform.position).magnitude <= 0.45f)
        {
            V1.transform.position -= 0.0022f * (V2.transform.position - V1.transform.position).normalized;
            V2.transform.position += 0.0044f * (V2.transform.position - V1.transform.position).normalized;
            move1 = true;
            move2 = true;
        }
    }

}
