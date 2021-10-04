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
    public ProcessResultCell cell0;
    public ProcessResultCell cell1;
    public ProcessResultCell cell2;
    public ProcessResultCell cell3;
    public ProcessResultCell cell4;
    public ProcessResultCell cell5;
    public ProcessResultCell cell6;
    public ProcessResultCell cell7;
    public ProcessResultCell cell8;

    public Text Title;
    public Text SuccessMessage;

    public Text ErrorTitle;

    public Button NextButton;
    public Button BackButton;

    List<QuantityError> quantityErrors = new List<QuantityError>();

    //bool MeasureErrorFlag = false;//直接测量量错误是否解决
    int curError = 0;

    public class QuantityError
    {
        public string Title { get; set; }
        public string Symbol { get; set; }

        public bool right { get; set; } = true;
        public QuantityErrorMessage a { get; set; } = new QuantityErrorMessage();//一元线性回归的a,b,r
        public QuantityErrorMessage b { get; set; } = new QuantityErrorMessage();
        public QuantityErrorMessage r { get; set; } = new QuantityErrorMessage();
        public QuantityErrorMessage average { get; set; } = new QuantityErrorMessage();
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
        quantityErrors = new List<QuantityError>();
        //MeasureErrorFlag = false;
        foreach (var item in RecordManager.tempRecord.quantities)
        {
            if (item.processMethod == 1)
            {
                UserInputTable calc = new UserInputTable();
                calc.varname = item.Symbol;
                List<double> temp = new List<double>();
                for (int i = 0; i < item.MesuredData.data.Count; i++)
                {
                    temp.Add(double.Parse(item.MesuredData.data[i]));
                }
                calc.data = new CalcVariable(GameManager.Instance.GetInstrument(item.InstrumentType).ErrorLimit / Math.Sqrt(3), temp.Count);
                calc.data.values = temp;
                calc.data.userua = item.UaExpression.GetExpressionExecuted();
                calc.data.userub = item.UbExpression.GetExpressionExecuted();
                calc.data.userunc = item.ComplexExpression.GetExpressionExecuted();
                CalcResult result = CalcResult.CheckTable(calc);
                if (!result.err.right)
                {
                    //MeasureErrorFlag = true;
                    if (!result.err.ua.right)
                    {
                        result.err.ua.userformula = item.UaExpression;
                    }
                    if (!result.err.ub.right)
                    {
                        result.err.ub.userformula = item.UbExpression;
                    }
                    if (!result.err.unc.right)
                    {
                        result.err.unc.userformula = item.ComplexExpression;
                    }
                    quantityErrors.Add(result.err);
                }
            }
            else if(item.processMethod == 2)
            {
                UserInputSuccessiveDifference calc = new UserInputSuccessiveDifference();
                double[] temp = new double[item.DifferencedData.data.Count];
                for (int i = 0; i < item.DifferencedData.data.Count; i++)
                {
                    temp[i] = double.Parse(item.DifferencedData.data[i]);
                }
                calc.name = item.Symbol;
                calc.y_nplusi_minus_y_i = temp;
                calc.x_nplusi_minus_x_i = double.Parse(item.stepLength);
                calc.user_aver_b = item.AverageExpression.GetExpressionExecuted();
                calc.correct_b_uncb = GameManager.Instance.GetInstrument(item.InstrumentType).ErrorLimit / Math.Sqrt(3);
                calc.user_b_unca = item.UaExpression.GetExpressionExecuted();
                calc.user_b_uncb = item.UbExpression.GetExpressionExecuted();
                calc.user_b_unc = item.ComplexExpression.GetExpressionExecuted();
                CalcResult result = CalcResult.CheckSuccessiveDifference(calc);
                if (!result.err.right)
                {
                    //MeasureErrorFlag = true;
                    if (!result.err.average.right)
                    {
                        result.err.average.userformula = item.AverageExpression;
                    }
                    if (!result.err.ua.right)
                    {
                        result.err.ua.userformula = item.UaExpression;
                    }
                    if (!result.err.ub.right)
                    {
                        result.err.ub.userformula = item.UbExpression;
                    }
                    if (!result.err.unc.right)
                    {
                        result.err.unc.userformula = item.ComplexExpression;
                    }
                    quantityErrors.Add(result.err);
                }
            }
            else if (item.processMethod == 3)
            {
                UserInputLinearRegression calc = new UserInputLinearRegression();
                calc.name = item.Symbol;
                double[] temp = new double[item.IndependentData.data.Count];
                for (int i = 0; i < item.IndependentData.data.Count; i++)
                {
                    temp[i] = double.Parse(item.IndependentData.data[i]);
                }
                calc.x = temp;
                temp = new double[item.MesuredData.data.Count];
                for (int i = 0; i < item.MesuredData.data.Count; i++)
                {
                    temp[i] = double.Parse(item.MesuredData.data[i]);
                }
                calc.y = temp;
                calc.a = item.AExpression.GetExpressionExecuted();
                calc.b = item.BExpression.GetExpressionExecuted();
                calc.r = item.RelationExpression.GetExpressionExecuted();
                calc.correct_uncb = GameManager.Instance.GetInstrument(item.InstrumentType).ErrorLimit / Math.Sqrt(3);
                calc.f_unca = item.UaExpression.GetExpressionExecuted();
                calc.f_uncb = item.UbExpression.GetExpressionExecuted();
                calc.f_unc = item.ComplexExpression.GetExpressionExecuted();
                calc.ifa = (item.nextValue != 0);
                CalcResult result = CalcResult.CheckRegression(calc);
                if (!result.err.right)
                {
                    //MeasureErrorFlag = true;
                    if (!result.err.a.right)
                    {
                        result.err.a.userformula = item.AExpression;
                    }
                    if (!result.err.b.right)
                    {
                        result.err.b.userformula = item.BExpression;
                    }
                    if (!result.err.r.right)
                    {
                        result.err.r.userformula = item.RelationExpression;
                    }
                    if (!result.err.ua.right)
                    {
                        result.err.ua.userformula = item.UaExpression;
                    }
                    if (!result.err.ub.right)
                    {
                        result.err.ub.userformula = item.UbExpression;
                    }
                    if (!result.err.unc.right)
                    {
                        result.err.unc.userformula = item.ComplexExpression;
                    }
                    quantityErrors.Add(result.err);
                }
            }
        }

        if (quantityErrors.Count > RecordManager.tempRecord.score.MeasureQuantityError)
            RecordManager.tempRecord.score.MeasureQuantityError = quantityErrors.Count;
        else if (quantityErrors.Count < RecordManager.tempRecord.score.MeasureQuantityError)
            RecordManager.tempRecord.score.MeasureQuantityError += quantityErrors.Count;

        CalcArgs calc_complex = new CalcArgs();
        foreach (var item in RecordManager.tempRecord.quantities)
        {
            calc_complex.AddVariable(item.Symbol, GameManager.Instance.GetInstrument(item.InstrumentType).ErrorLimit / Math.Sqrt(3), item.MesuredData.data.Count);
            calc_complex.Measure(item.Symbol, item.MesuredData.data.ToDouble().ToArray());
            calc_complex.MeasureUserUnput(item.Symbol, item.UaExpression.GetExpressionExecuted(), item.UbExpression.GetExpressionExecuted(), item.ComplexExpression.GetExpressionExecuted());
        }
        CheckComplex(calc_complex);

        /*CalcArgs calc = new CalcArgs();
        quantityErrors = new List<QuantityError>();

        CheckMeasuredUncertainty(calc);
        CheckMeasuredDifferenced(calc);
        CheckMeasuredRegression(calc);

        var measureresult = CalcArgs.CalculateMeasureValue(calc);

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

        CheckComplex(calc);*/
    }

    private void CheckComplex(CalcArgs calc)
    {
        //if (!MeasureErrorFlag)
        //{
            calc.ComplexUserUnput(RecordManager.tempRecord.complexQuantityModel.AverageExpression.GetExpressionExecuted(), RecordManager.tempRecord.complexQuantityModel.UncertainExpression.GetExpressionExecuted());
            var complexresult = CalcArgs.CalculateComplexValue(RecordManager.tempRecord.stringExpression, calc);
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
        //}
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
            if(curError == quantityErrors.Count - 1)
                NextButton.GetComponentInChildren<Text>().text = "查看评分";
            var current = quantityErrors[curError];
            Title.text = $"你的错误{curError + 1}/{quantityErrors.Count}";
            ErrorTitle.text = current.Title;

            if (!current.b.right)
            {
                cell0.gameObject.SetActive(true);
                cell0.ShowData("一元线性回归系数b计算有误", current.b.message, current.b.latex, current.b.userformula);
            }
            else
            {
                cell0.gameObject.SetActive(false);
            }
            if (!current.a.right)
            {
                cell1.gameObject.SetActive(true);
                cell1.ShowData("一元线性回归系数a计算有误", current.a.message, current.a.latex, current.a.userformula);
            }
            else
            {
                cell1.gameObject.SetActive(false);
            }           
            if (!current.r.right)
            {
                cell2.gameObject.SetActive(true);
                cell2.ShowData("相关系数r计算有误", current.r.message, current.r.latex, current.r.userformula);
            }
            else
            {
                cell2.gameObject.SetActive(false);
            }
            if (!current.average.right)
            {
                cell3.gameObject.SetActive(true);
                cell3.ShowData("平均值计算有误", current.average.message, current.average.latex, current.average.userformula);
            }
            else
            {
                cell3.gameObject.SetActive(false);
            }
            if (!current.ua.right)
            {
                cell4.gameObject.SetActive(true);
                cell4.ShowData("A类不确定度计算有误", current.ua.message, current.ua.latex, current.ua.userformula);
            }
            else
            {
                cell4.gameObject.SetActive(false);
            }
            if (!current.ub.right)
            {
                cell5.gameObject.SetActive(true);
                cell5.ShowData("B类不确定度计算有误", current.ub.message, current.ub.latex, current.ub.userformula);
            }
            else
            {
                cell5.gameObject.SetActive(false);
            }
            if (!current.unc.right)
            {
                cell6.gameObject.SetActive(true);
                cell6.ShowData("合成不确定度有误", current.unc.message, current.unc.latex, current.unc.userformula);
            }
            else
            {
                cell6.gameObject.SetActive(false);
            }
            if (!current.answer.right)
            {
                cell7.gameObject.SetActive(true);
                cell7.ShowData("最终合成结果计算有误", current.answer.message, current.answer.latex, current.answer.userformula);
            }
            else
            {
                cell7.gameObject.SetActive(false);
            }
            if (!current.answerunc.right)
            {
                cell8.gameObject.SetActive(true);
                cell8.ShowData("最终合成不确定度计算有误", current.answerunc.message, current.answerunc.latex, current.answerunc.userformula);
            }
            else
            {
                cell8.gameObject.SetActive(false);
            }
            curError++;
        }
        //else if (MeasureErrorFlag)
        //{
        //    UIAPI.Instance.ShowModel(new ModelDialogModel()
        //    {
        //        ShowCancel = false,
        //        Title = new BindableString("警告"),
        //        Message = new BindableString("请先修正直接测量量的错误")
        //    });
        //}
        else if (curError == quantityErrors.Count)
        {
            cell0.gameObject.SetActive(false);
            cell1.gameObject.SetActive(false);
            cell2.gameObject.SetActive(false);
            cell3.gameObject.SetActive(false);
            cell4.gameObject.SetActive(false);
            cell5.gameObject.SetActive(false);
            cell6.gameObject.SetActive(false);
            cell7.gameObject.SetActive(false);
            cell8.gameObject.SetActive(false);
            double score = RecordManager.tempRecord.score.CalcScore();
            Title.text = "本次实验得分:" + string.Format("{0:F1}", score) + "/4.0";
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
            Main.m_UI.OpenTemporaryUI<SaveRecordUILogic>();
        }
    }
}
