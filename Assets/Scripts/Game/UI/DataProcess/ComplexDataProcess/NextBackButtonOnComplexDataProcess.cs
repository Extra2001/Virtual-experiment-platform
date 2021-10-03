using HT.Framework;
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
            (flag, reason) = StaticMethods.CheckUncertain(RecordManager.tempRecord.complexQuantityModel.AnswerAverage, RecordManager.tempRecord.complexQuantityModel.AnswerUncertain);
            var rec = RecordManager.tempRecord;
            if (rec.complexQuantityModel.AverageExpression == null || rec.complexQuantityModel.AverageExpression.Count == 0)
            {
                ShowModel($"合成物理量的主值还未计算");
                return;
            }
            if (rec.complexQuantityModel.UncertainExpression == null || rec.complexQuantityModel.UncertainExpression.Count == 0)
            {
                ShowModel($"合成物理量的不确定度还未计算");
                return;
            }
        }
        catch
        {
            ShowModel($"结果最终表述有误，请重新输入");
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
        UIAPI.Instance.ShowModel(new ModelDialogModel()
        {
            ShowCancel = false,
            Title = new BindableString("错误"),
            Message = new BindableString(message)
        });
    }
}
