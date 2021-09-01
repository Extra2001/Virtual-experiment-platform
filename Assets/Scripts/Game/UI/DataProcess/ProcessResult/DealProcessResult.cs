/************************************************************************************
    作者：张峻凡、荆煦添、曹北健
    描述：计算并比对最终数据结果的程序
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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

    private void CheckRightOrWrong()
    {
        CalcArgs calc = new CalcArgs();
        QuantityError ComplexErr = new QuantityError();
        foreach (var item in RecordManager.tempRecord.quantities)
        {
            calc.AddVariable(item.Symbol, GameManager.Instance.GetInstrument(item.InstrumentType).ErrorLimit / System.Math.Sqrt(3), item.Groups);
            calc.Measure(item.Symbol, item.Data.ToDouble().ToArray());
            calc.MeasureUserUnput(item.Symbol, item.UaExpression.GetExpressionExecuted(), item.UbExpression.GetExpressionExecuted(), item.ComplexExpression.GetExpressionExecuted());
        }

        measureresult = CalcArgs.CalculateMeasureValue(calc);
        quantityErrors = new List<QuantityError>();
        bool flag = true;
        foreach (var item in measureresult.err)
        {
            if (item.Message != "计算正确")
            {
                flag = false;
                quantityErrors.Add(item);
            }

        }
        if (!flag)
        {
            ComplexErr.Title = "最终合成量计算有误";
            ComplexErr.Message = "直接测量量处理结果有误，请先修正直接测量量错误再处理最终合成量";
            quantityErrors.Add(ComplexErr);
        }
        else
        {
            calc.ComplexUserUnput(RecordManager.tempRecord.complexQuantityModel.AverageExpression.GetExpressionExecuted(), RecordManager.tempRecord.complexQuantityModel.UncertainExpression.GetExpressionExecuted());
            complexresult = CalcArgs.CalculateComplexValue(RecordManager.tempRecord.stringExpression, calc);
            if(complexresult.status != "计算无误")
            {
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
        //DealMeausredQuantities();
        //DealComplexData();
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
                    //formula3.FindChildren("ExpressionImage").GetComponent<Image>().FitHeight(res);
                });
            }
            else
            {
                formula5.SetActive(false);
            }

            /*if (current.Right != null)
            {
                RightFormula.gameObject.SetActive(true);
                RightFormula.LoadFormula(current.Right);
            }
            else RightFormula.gameObject.SetActive(false);*/
            curError++;
        }
        else if (curError == quantityErrors.Count)
        {
            formula1.SetActive(false);
            formula2.SetActive(false);
            formula3.SetActive(false);
            formula4.SetActive(false);
            formula5.SetActive(false);
            double score = RecordManager.tempRecord.score.CalcScore();
            Title.text = "本次实验得分:" + score + "/4.0";
            if (score - 4 == 0)
            {
                SuccessMessage.text = "你的实验已全部正确地完成！恭喜你掌握了本次误差理论的知识。";
            }
            else
            {
                SuccessMessage.text = "本次实验中，\n";
                SuccessMessage.text += "数据记录过程犯了" + RecordManager.tempRecord.score.DataRecordError + "次错误\n";
                SuccessMessage.text += "直接测量量数据处理过程犯了" + RecordManager.tempRecord.score.MeasureQuantityError + "次错误\n";
                SuccessMessage.text += "合成量数据处理过程犯了" + RecordManager.tempRecord.score.ComplexQuantityError + "次错误\n";
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
