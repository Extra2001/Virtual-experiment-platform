/************************************************************************************
    作者：张柯
    描述：温度计行为处理程序
*************************************************************************************/
using HT.Framework;
using DG.Tweening;

public class thermometer_main : HTBehaviour
{
    // Start is called before the first frame update、
    public void ShowNum(float num)
    {
        transform.Find("Cylinder001").transform.DOScaleZ(0.67f + num * 1.1f / 50f, 1f).SetEase(Ease.OutExpo);
    }


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
