/************************************************************************************
    ���ߣ�������
    ��������ʡ��ʾ������Դ����̬���صƹ��������Ⱦ��
*************************************************************************************/
using System.Runtime.InteropServices;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void _SetTips(string tips);
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
