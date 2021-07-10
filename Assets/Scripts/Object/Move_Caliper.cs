/************************************************************************************
    作者：张柯
    描述：游标卡尺行为处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;

public class Move_Caliper : HTBehaviour
{
    public float num;
    public void Movenum(float num)
    {
        this.transform.localPosition -= new Vector3(0.02f*num, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movenum(num);

    }
}
