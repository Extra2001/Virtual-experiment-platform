/************************************************************************************
    作者：张峻凡
    描述：下一步上一步点击处理程序
*************************************************************************************/
using HT.Framework;
using System.Linq;
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
        bool flag = true;
        foreach(var item in RecordManager.tempRecord.quantities)
        {
            if (item.MesuredData.data.Count == 0)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString($"物理量 {item.Name}({item.Symbol}) 还没记录数据")
                });
                flag = false;break;
            }
            if (item.MesuredData.data.Where(x => string.IsNullOrEmpty(x)).Count() != 0)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString($"物理量 {item.Name}({item.Symbol}) 存在空数据")
                });
                flag = false; break;
            }
        }
        if(flag) GameManager.Instance.SwitchNextProcedure();
    }
}
