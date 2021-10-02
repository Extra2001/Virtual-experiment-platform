/************************************************************************************
    作者：张峻凡
    描述：下一步上一步点击处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class NextBackButtonOnProcessResult : HTBehaviour
{
    public Button BackButton;
    public Button EndButton;

    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(Back);
        EndButton.onClick.AddListener(End);
    }

    public void Back()
    {
        GameManager.Instance.SwitchBackProcedure();
    }

    public void End()
    {
        
    }
}
