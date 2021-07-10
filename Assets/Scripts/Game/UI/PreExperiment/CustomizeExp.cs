/************************************************************************************
    作者：荆煦添
    描述：自定义实验处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class CustomizeExp : HTBehaviour
{
    public int ExpId;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Main.m_Event.Throw(Main.m_ReferencePool.Spawn<ChooseExpEventHandler>().Fill(ExpId));
        });
    }
}
