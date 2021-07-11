/************************************************************************************
    作者：张柯
    描述：螺旋测微器行为处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;
//螺旋测微计旋转效应
//注：为了防止在旋钮中触碰物体导致可能的问题，将旋转独立执行
//采用旋钮模拟方式完成
public class Rotate_micrometer : HTBehaviour
{
    public float num;
    private int prenum;
    public void Rotatenum(float num)
    {
        num = num / 5.0f;

            this.transform.localPosition -= new Vector3(0, (0.53f * num) / 5000f, 0);

            //this.transform.parent.Find("对象014").Find("Camera").localPosition -= new Vector3(0, (0.53f*num)/5000f, 0);
            //神秘bug

            this.transform.Rotate(new Vector3(0, num / 50.0f * 360.0f, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotatenum(num);
    }
}
