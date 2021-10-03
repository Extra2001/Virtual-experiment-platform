/************************************************************************************
    作者：张峻凡
    描述：下一步上一步点击处理程序
*************************************************************************************/
using HT.Framework;
using System.Linq;
using UnityEngine.UI;

public class NextBackButtonOnProcessExplain : HTBehaviour
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
        if (CheckAll())
        {
            GameManager.Instance.SwitchProcedure<MeasuredDataProcessProcedure>();
            GameManager.Instance._currentQuantityIndex = 0;
            GameManager.Instance.ShowUncertainty();
        }
    }

    public bool CheckAll(bool silent = false)
    {
        foreach (var item in RecordManager.tempRecord.quantities)
        {
            if (item.MesuredData.data.Count == 0)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = $"物理量 {item.Name}({item.Symbol}) 还没记录数据"
                });
                return false;
            }
            if (item.MesuredData.data.Where(x => string.IsNullOrEmpty(x)).Count() != 0)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Message = $"物理量 {item.Name}({item.Symbol}) 存在空数据"
                });
                return false;
            }
        }
        return true;
    }
}
