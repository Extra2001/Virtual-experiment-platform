using HT.Framework;
using System.Linq;
using UnityEngine.UI;

public class NextBackButtonOnComplexDataProcess : HTBehaviour
{
    public Button BackButton;
    public Button NextButton;

    private void Start()
    {
        BackButton.onClick.AddListener(GameManager.Instance.SwitchBackProcedure);
        NextButton.onClick.AddListener(Next);
    }

    private void Next()
    {
        bool flag;
        string reason;
        try
        {
            var rec = RecordManager.tempRecord;
            if (rec.complexQuantityModel.AverageExpression == null || rec.complexQuantityModel.AverageExpression.Count == 0)
            {
                ShowModel($"�ϳ�����������ֵ��δ����");
                return;
            }
            if (RecordManager.tempRecord.quantities.Where(x => x.processMethod == 4).Any())
            {
                flag = true;
                reason = "";
            }
            else
            {
                if (rec.complexQuantityModel.UncertainExpression == null || rec.complexQuantityModel.UncertainExpression.Count == 0)
                {
                    ShowModel($"�ϳ��������Ĳ�ȷ���Ȼ�δ����");
                    return;
                }
                (flag, reason) = StaticMethods.CheckUncertain(RecordManager.tempRecord.complexQuantityModel.AnswerAverage, RecordManager.tempRecord.complexQuantityModel.AnswerUncertain);
            }
        }
        catch
        {
            ShowModel($"������ձ�����������������");
            return;
        }
        if (flag)
        {
            GameManager.Instance.SwitchProcedure<ProcessResultProcedure>();
        }
        else
        {
            RecordManager.tempRecord.score.ComplexQuantityError += 1;
            ShowModel(reason);
        }
    }

    private void ShowModel(string message)
    {
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            ShowCancel = false,
            Title = "����",
            Message = message
        });
    }
}
