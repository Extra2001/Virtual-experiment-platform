/************************************************************************************
    ���ߣ�������
    ��������ʡ��ʾ������Դ����̬���صƹ��������Ⱦ��
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
