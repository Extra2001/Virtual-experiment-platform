/************************************************************************************
    作者：荆煦添
    描述：节省显示计算资源，动态开关灯光和内饰渲染。
*************************************************************************************/
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    public static RenderManager Instance;
    public GameObject BuiltIn;
    public GameObject Light;

    private void Start()
    {
        Instance = this;
    }

    public void Show()
    {
        BuiltIn.SetActive(true);
        Light.SetActive(true);
    }

    public void Hide()
    {
        BuiltIn.SetActive(false);
        Light.SetActive(false);
    }
}
