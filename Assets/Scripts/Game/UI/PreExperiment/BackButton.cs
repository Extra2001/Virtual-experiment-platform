/************************************************************************************
    作者：荆煦添
    描述：上一步按钮处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class BackButton : HTBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(GameManager.Instance.SwitchBackProcedure);
    }
}
