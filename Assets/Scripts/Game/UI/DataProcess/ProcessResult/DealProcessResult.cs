/************************************************************************************
    作者：张峻凡、荆煦添、曹北健
    描述：计算并比对最终数据结果的程序
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;

public class DealProcessResult : HTBehaviour
{
    public ProcessResultCell cell1;
    public ProcessResultCell cell2;
    public ProcessResultCell cell3;
    public ProcessResultCell cell4;
    public ProcessResultCell cell5;

    public Text Title;
    public Text SuccessMessage;

    public Text ErrorTitle;

    public Button NextButton;
    public Button BackButton;


    List<QuantityError> quantityErrors = new List<QuantityError>();


    bool MeasureErrorFlag = false;//直接测量量错误是否解决
    int curError = 0;
    CalcMeasureResult measureresult = new CalcMeasureResult();
    CalcComplexResult complexresult = new CalcComplexResult();

    public class QuantityError
    {
        public string Title { get; set; }
        public string Symbol { get; set; }

        public bool right { get; set; } = true;
        public QuantityErrorMessage ua { get; set; } = new QuantityErrorMessage();
        public QuantityErrorMessage ub { get; set; } = new QuantityErrorMessage();
        public QuantityErrorMessage unc { get; set; } = new QuantityErrorMessage();
        public QuantityErrorMessage answer { get; set; } = new QuantityErrorMessage();
        public QuantityErrorMessage answerunc { get; set; } = new QuantityErrorMessage();

    }

    public class QuantityErrorMessage
    {
        public bool right { get; set; } = true;
        public List<FormulaNode> userformula { get; set; }
        public string latex { get; set; }//latex公式
        public string message { get; set; }//错误信息
    }

    private void Start()
    {
        NextButton.onClick.AddListener(Next);
        BackButton.onClick.AddListener(() =>
        {
            if (curError <= 1)
                GameManager.Instance.SwitchBackProcedure();
            else
            {
                curError -= 2;
                Next();
            }
        });
    }

    private void CheckRightOrWrong()
    {
        CalcArgs calc = new CalcArgs();
        QuantityError ComplexErr = new QuantityError();

        foreach (var item in RecordManager.tempRecord.quantities)
        {
            calc.AddVariable(item.Symbol, GameManager.Instance.GetInstrument(item.InstrumentType).ErrorLimit / Math.Sqrt(3), item.Groups);
            calc.Measure(item.Symbol, item.Data.ToDouble().ToArray());
            calc.MeasureUserUnput(item.Symbol, item.UaExpression.GetExpressionExecuted(), item.UbExpression.GetExpressionExecuted(), item.ComplexExpression.GetExpressionExecuted());
        }
        quantityErrors = new List<QuantityError>();
        measureresult = CalcArgs.CalculateMeasureValue(calc);

        MeasureErrorFlag = false;
        foreach (var item in measureresult.err)
        {
            if (!item.right)
            {
                if (!item.ua.right)
                {
                    item.ua.userformula = RecordManager.tempRecord.quantities.Where(x => x.Symbol.Equals(item.Symbol)).FirstOrDefault().UaExpression;
                }
                if (!item.ub.right)
                {
                    item.ub.userformula = RecordManager.tempRecord.quantities.Where(x => x.Symbol.Equals(item.Symbol)).FirstOrDefault().UbExpression;
                }
                if (!item.unc.right)
                {
                    item.unc.userformula = RecordManager.tempRecord.quantities.Where(x => x.Symbol.Equals(item.Symbol)).FirstOrDefault().ComplexExpression;
                }
                MeasureErrorFlag = true;
                quantityErrors.Add(item);
            }

        }

        if (quantityErrors.Count > RecordManager.tempRecord.score.MeasureQuantityError)
            RecordManager.tempRecord.score.MeasureQuantityError = quantityErrors.Count;
        else if (quantityErrors.Count < RecordManager.tempRecord.score.MeasureQuantityError)
            RecordManager.tempRecord.score.MeasureQuantityError += quantityErrors.Count;

        if (!MeasureErrorFlag)
        {
            calc.ComplexUserUnput(RecordManager.tempRecord.complexQuantityModel.AverageExpression.GetExpressionExecuted(), RecordManager.tempRecord.complexQuantityModel.UncertainExpression.GetExpressionExecuted());
            complexresult = CalcArgs.CalculateComplexValue(RecordManager.tempRecord.stringExpression, calc);
            if (complexresult.status != "计算无误")
            {                
                if (!complexresult.err.answer.right)
                {
                    RecordManager.tempRecord.score.ComplexQuantityError += 1;
                    complexresult.err.answer.userformula = RecordManager.tempRecord.complexQuantityModel.AverageExpression;
                }
                if (!complexresult.err.answerunc.right)
                {
                    RecordManager.tempRecord.score.ComplexQuantityError += 1;
                    complexresult.err.answerunc.userformula = RecordManager.tempRecord.complexQuantityModel.UncertainExpression;
                }
                quantityErrors.Add(complexresult.err);
            }

        }


    }

    public void Show()
    {
        CheckRightOrWrong();
        curError = 0;
        Next();
    }

    public void Next()
    {
        if (curError < quantityErrors.Count)
        {
            ErrorTitle.gameObject.SetActive(true);
            SuccessMessage.gameObject.SetActive(false);
            BackButton.GetComponentInChildren<Text>().text = "上一个";
            NextButton.GetComponentInChildren<Text>().text = "下一个";
            var current = quantityErrors[curError];
            Title.text = $"你的错误{curError + 1}/{quantityErrors.Count}";
            ErrorTitle.text = current.Title;

            if (!current.ua.right)
            {
                cell1.gameObject.SetActive(true);
                cell1.ShowData("A类不确定度计算有误", current.ua.message, current.ua.latex, current.ua.userformula);
            }
            else
            {
                cell1.gameObject.SetActive(false);
            }
            if (!current.ub.right)
            {
                cell2.gameObject.SetActive(true);
                cell2.ShowData("B类不确定度计算有误", current.ub.message, current.ub.latex, current.ub.userformula);
            }
            else
            {
                cell2.gameObject.SetActive(false);
            }
            if (!current.unc.right)
            {
                cell3.gameObject.SetActive(true);
                cell3.ShowData("合成不确定度有误", current.unc.message, current.unc.latex, current.unc.userformula);
            }
            else
            {
                cell3.gameObject.SetActive(false);
            }
            if (!current.answer.right)
            {
                cell4.gameObject.SetActive(true);
                cell4.ShowData("最终合成结果计算有误", current.answer.message, current.answer.latex, current.answer.userformula);
            }
            else
            {
                cell4.gameObject.SetActive(false);
            }
            if (!current.answerunc.right)
            {
                cell5.gameObject.SetActive(true);
                cell5.ShowData("最终合成不确定度计算有误", current.answerunc.message, current.answerunc.latex, current.answerunc.userformula);
            }
            else
            {
                cell5.gameObject.SetActive(false);
            }

            curError++;
        }
        else if (MeasureErrorFlag)
        {
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                ShowCancel = false,
                Title = new BindableString("警告"),
                Message = new BindableString("请先修正直接测量量的错误")
            });
        }
        else if (curError == quantityErrors.Count)
        {
            cell1.gameObject.SetActive(false);
            cell2.gameObject.SetActive(false);
            cell3.gameObject.SetActive(false);
            cell4.gameObject.SetActive(false);
            cell5.gameObject.SetActive(false);
            double score = RecordManager.tempRecord.score.CalcScore();
            Title.text = "本次实验得分:" + String.Format("{0:F1}", score) + "/4.0";
            if (score - 4 == 0)
            {
                SuccessMessage.text = "你的实验已全部正确地完成！恭喜你掌握了本次误差理论的知识。";
            }
            else
            {
                SuccessMessage.text = "本次实验中，\n";
                if (RecordManager.tempRecord.score.DataRecordError > 0)
                {
                    SuccessMessage.text += "数据记录过程犯了" + RecordManager.tempRecord.score.DataRecordError + "次错误\n";
                }
                else if (RecordManager.tempRecord.score.MeasureQuantityError > 0)
                {
                    SuccessMessage.text += "直接测量量数据处理过程犯了" + RecordManager.tempRecord.score.MeasureQuantityError + "次错误\n";
                }
                else if (RecordManager.tempRecord.score.ComplexQuantityError > 0)
                {
                    SuccessMessage.text += "合成量数据处理过程犯了" + RecordManager.tempRecord.score.ComplexQuantityError + "次错误\n";
                }
                SuccessMessage.text += "请牢记错误原因，以免下次再犯";
            }

            SuccessMessage.text += "\n\n你可以保存此次实验，以便将来回顾。\n\n保存后，你可以选择分享此存档给同学。";
            SuccessMessage.gameObject.SetActive(true);
            ErrorTitle.gameObject.SetActive(false);
            BackButton.GetComponentInChildren<Text>().text = "返回";
            NextButton.GetComponentInChildren<Text>().text = "保存实验";
            curError++;
        }
        else
        {
            PauseManager.Instance.Pause();
            Main.m_UI.OpenTemporaryUI<SaveRecordUILogic>();
        }
    }
}
