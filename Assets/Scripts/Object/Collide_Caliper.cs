/************************************************************************************
    作者：张柯
    描述：游标卡尺行为处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;

public class Collide_Caliper : HTBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        if ((collision.collider.tag == "Mesured") && this.transform.parent.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num <= -0.01f)
        {
            this.transform.parent.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num += 0.05f;
        }
        if (this.transform.parent.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num <= 0.0001f && this.transform.parent.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num >= -0.0001f)
        {
            this.transform.parent.transform.Find("MeasureHead").gameObject.GetComponent<Move_Caliper>().num = 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
