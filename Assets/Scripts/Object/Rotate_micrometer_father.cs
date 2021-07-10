/************************************************************************************
    作者：张柯
    描述：螺旋测微器行为处理程序
*************************************************************************************/
using HT.Framework;

public class Rotate_micrometer_father : HTBehaviour
{
    public int RotatePerTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Find("Micrometer_grandson").Find("Obj3d66-1078630-1-677").GetComponent<Rotate_micrometer>().Rotatenum(RotatePerTime);
    }
}
