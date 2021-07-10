/************************************************************************************
    作者：张峻凡
    描述：下一步上一步点击处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class NextBackButtonControl : HTBehaviour
{
    public Button BackButton;
    public Button NextButton;

    void Start()
    {
        BackButton.onClick.AddListener(Back);
        NextButton.onClick.AddListener(Next);
    }

    public void Back()
    {
        GameManager.Instance.SwitchBackProcedure();
    }
    public void Next()
    {
        GameManager.Instance.SwitchNextProcedure();
    }
}
