/************************************************************************************
    作者：张柯
    描述：螺旋测微器行为处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;

public class Collide : HTBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        if ((collision.collider.tag == "Mesured") && this.transform.parent.transform.Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num<=-0.5f)
        {
            this.transform.parent.transform.Find("rotatebody_main").gameObject.GetComponent<Rotate_micrometer>().num += 0.5f;
        }
    }
}
