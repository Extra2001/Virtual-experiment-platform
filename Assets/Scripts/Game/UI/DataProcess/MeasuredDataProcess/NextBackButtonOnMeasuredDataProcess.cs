/************************************************************************************
    ���ߣ��ž���
    ���������㲻ȷ������һ����һ������
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class NextBackButtonOnMeasuredDataProcess : HTBehaviour
{
    public Button NextButton;
    public Button BackButton;
    public MeasuredProcessController controller;

    private void Start()
    {
        GameManager gm = GameManager.Instance;
        BackButton.onClick.AddListener(() =>
        {
            if (gm._currentQuantityIndex == 0)
            {
                Main.m_UI.CloseUI<MeasuredDataProcess>();
                gm.SwitchBackProcedure();
            }
            else
            {
                gm._currentQuantityIndex--;
                gm.ShowUncertainty();
            }
        });
        NextButton.onClick.AddListener(() =>
        {
            if (controller != null && controller.CheckAll())
            {
                if (gm._currentQuantityIndex >= RecordManager.tempRecord.quantities.Count - 1)
                {
                    gm.SwitchProcedure<ComplexDataProcessProcedure>();
                }
                else
                {
                    gm._currentQuantityIndex++;
                    gm.ShowUncertainty();
                }
            }
        });
    }
}
