/************************************************************************************
    作者：张峻凡、荆煦添、曹北健
    描述：计算并比对最终数据结果的程序
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DealProcessResult : HTBehaviour
{
    public GameObject formula1;
    public GameObject formula2;
    public GameObject formula3;
    public GameObject formula4;
    public GameObject formula5;

    public Text Title;
    public Text SuccessMessage;

    public Text ErrorTitle;
    public Text ErrorMessage;

    public Button NextButton;
    public Button BackButton;

    //Dictionary<QuantityModel, DataChart> quantities = new Dictionary<QuantityModel, DataChart>();

    List<QuantityError> quantityErrors = new List<QuantityError>();

    double ComplexAverage = 0, ComplexUncertainty = 0, UserComplexAverage = 0, UserComplexUncertainty;

    bool MeasureErrorFlag = false;//直接测量量错误是否解决
    int curError = 0;
    CalcMeasureResult measureresult = new CalcMeasureResult();
    CalcComplexResult complexresult = new CalcComplexResult();

    public class QuantityError
    {
        public string Title { get; set; }
        public string Message { get; set; }
        //直接测量量的ua,ub,unc
        public string ua { get; set; }
        public string ub { get; set; }
        public string unc { get; set; }

        public string answer { get; set; }
        public string answerunc { get; set; }
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

    private string double2string(double value)
    {
        var str = value.ToString();
        if (str.Contains("."))
            str = str.TrimEnd('0');
        if (str.EndsWith("."))
            str = str.Remove(str.Length - 1, 1);
        return str;
    }

    private void CheckRightOrWrong()
    {
        CalcArgs calc = new CalcArgs();
        QuantityError ComplexErr = new QuantityError();
        List<QuantityError> recordErrors = new List<QuantityError>();

        foreach (var item in RecordManager.tempRecord.quantities)
        {
            calc.AddVariable(item.Symbol, GameManager.Instance.GetInstrument(item.InstrumentType).ErrorLimit / System.Math.Sqrt(3), item.Groups);
            calc.Measure(item.Symbol, item.Data.ToDouble().ToArray());
            calc.MeasureUserUnput(item.Symbol, item.UaExpression.GetExpressionExecuted(), item.UbExpression.GetExpressionExecuted(), item.ComplexExpression.GetExpressionExecuted());

            var instrument = item.InstrumentType.CreateInstrumentInstance();

            // 以下判断有效数字位数
            foreach (var subitem in item.Data)
            {
                var (res, answer) = instrument.CheckErrorLimit(subitem);
                if (!res)
                {
                    recordErrors.Add(new QuantityError()
                    {
                        answer = answer,
                        answerunc = subitem,
                        Message = "读取数据有效数字位数与仪器不匹配。",
                        Title = "有效数字位数错误"
                    });
                }
            }
        }
        quantityErrors = new List<QuantityError>();
        measureresult = CalcArgs.CalculateMeasureValue(calc);

        foreach (var item in measureresult.err)
        {
            if (item.Message != "计算正确")
            {
                MeasureErrorFlag = true;
                quantityErrors.Add(item);
            }
            else
            {
                MeasureErrorFlag = false;
            }
        }

        if (recordErrors.Count > RecordManager.tempRecord.score.DataRecordError)
            RecordManager.tempRecord.score.DataRecordError = recordErrors.Count;
        else if (recordErrors.Count < RecordManager.tempRecord.score.DataRecordError)
            RecordManager.tempRecord.score.DataRecordError += recordErrors.Count;

        if (quantityErrors.Count > RecordManager.tempRecord.score.MeasureQuantityError)
            RecordManager.tempRecord.score.MeasureQuantityError = quantityErrors.Count;
        else if (quantityErrors.Count < RecordManager.tempRecord.score.MeasureQuantityError)
            RecordManager.tempRecord.score.MeasureQuantityError += quantityErrors.Count;

        if (MeasureErrorFlag)
        {
            ComplexErr.Title = "最终合成量计算有误";
            ComplexErr.Message = "\n\n直接测量量处理结果有误，请先修正直接测量量错误再处理最终合成量";
            quantityErrors.Add(ComplexErr);
        }
        else
        {
            calc.ComplexUserUnput(RecordManager.tempRecord.complexQuantityModel.AverageExpression.GetExpressionExecuted(), RecordManager.tempRecord.complexQuantityModel.UncertainExpression.GetExpressionExecuted());
            complexresult = CalcArgs.CalculateComplexValue(RecordManager.tempRecord.stringExpression, calc);
            if (complexresult.status != "计算无误")
            {
                RecordManager.tempRecord.score.ComplexQuantityError += 1;
                ComplexErr.Title = complexresult.err.Title;
                ComplexErr.Message = complexresult.err.Message;
                ComplexErr.answer = complexresult.calcexpr.ToLaTeX();
                ComplexErr.answerunc = complexresult.uncexpr.ToLaTeX();
                quantityErrors.Add(ComplexErr);
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
            ErrorMessage.gameObject.SetActive(true);
            ErrorTitle.gameObject.SetActive(true);
            SuccessMessage.gameObject.SetActive(false);
            BackButton.GetComponentInChildren<Text>().text = "上一个";
            NextButton.GetComponentInChildren<Text>().text = "下一个";
            var current = quantityErrors[curError];
            Title.text = $"你的错误{curError + 1}/{quantityErrors.Count}";
            ErrorTitle.text = current.Title;
            ErrorMessage.text = current.Message;

            if (current.ua != null)
            {
                formula1.SetActive(true);
                LatexEquationRender.Render(current.ua, res =>
                {
                    formula1.FindChildren("ExpressionImage").GetComponent<Image>().sprite = res;
                });
            }
            else
            {
                formula1.SetActive(false);
            }
            if (current.ub != null)
            {
                formula2.SetActive(true);
                LatexEquationRender.Render(current.ub, res =>
                {
                    formula2.FindChildren("ExpressionImage").GetComponent<Image>().sprite = res;
                });
            }
            else
            {
                formula2.SetActive(false);
            }
            if (current.unc != null)
            {
                formula3.SetActive(true);
                LatexEquationRender.Render(current.unc, res =>
                {
                    formula3.FindChildren("ExpressionImage").GetComponent<Image>().sprite = res;
                    //formula3.FindChildren("ExpressionImage").GetComponent<Image>().FitHeight(res);
                });
            }
            else
            {
                formula3.SetActive(false);
            }
            if (current.answer != null)
            {
                formula4.SetActive(true);
                LatexEquationRender.Render(current.answer, res =>
                {
                    formula4.FindChildren("ExpressionImage").GetComponent<Image>().sprite = res;
                });
            }
            else
            {
                formula4.SetActive(false);
            }
            if (current.answerunc != null)
            {
                formula5.SetActive(true);
                LatexEquationRender.Render(current.answerunc, res =>
                {
                    formula5.FindChildren("ExpressionImage").GetComponent<Image>().sprite = res;
                });
            }
            else
            {
                formula5.SetActive(false);
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
            formula1.SetActive(false);
            formula2.SetActive(false);
            formula3.SetActive(false);
            formula4.SetActive(false);
            formula5.SetActive(false);
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
                    SuccessMessage.text += "数据记录过程犯了" + RecordManager.tempRecord.score.MeasureQuantityError + "次错误\n";
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
            //Title.text = quantityErrors.Count == 0 ? "正确无误！" : "你已查看了所有的错误！";
            //SuccessMessage.text = quantityErrors.Count == 0 ? "你的实验已全部正确地完成！恭喜你掌握了本次误差理论的知识。" :
            //    "你数据处理中所有的错误如上，请认真记录并思考。";
            SuccessMessage.text += "\n\n你可以保存此次实验，以便将来回顾。\n\n保存后，你可以选择分享此存档给同学。";
            SuccessMessage.gameObject.SetActive(true);
            ErrorTitle.gameObject.SetActive(false);
            ErrorMessage.gameObject.SetActive(false);
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
