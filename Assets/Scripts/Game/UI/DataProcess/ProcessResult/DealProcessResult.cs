using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DealProcessResult : HTBehaviour
{
    public FormulaController WrongFormula;
    public FormulaController RightFormula;

    public Text Title;
    public Text SuccessMessage;

    public Text ErrorTitle;
    public Text ErrorMessage;

    public Button NextButton;
    public Button BackButton;

    Dictionary<QuantityModel, DataChart> quantities = new Dictionary<QuantityModel, DataChart>();

    List<QuantityError> quantityErrors = new List<QuantityError>();

    double ComplexAverage = 0, ComplexUncertainty = 0, UserComplexAverage = 0, UserComplexUncertainty;

    int curError = 0;

    public class QuantityError
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public List<FormulaNode> User { get; set; }
        public List<FormulaNode> Right { get; set; }
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

    private void DealMeausredQuantities()
    {
        quantities.Clear();
        quantityErrors.Clear();

        double avg, ua, u;
        foreach (var item in RecordManager.tempRecord.quantities)
        {
            var Ub = Main.m_Entity.GetEntities(item.InstrumentType).First().Cast<InstrumentBase>().ErrorLimit / System.Math.Sqrt(3);
            (avg, ua, u) = StaticMethods.CalcUncertain(item.Data, Ub);
            var data = new DataChart()
            {
                Name = item.Symbol,
                Average = avg,
                Ua = ua,
                Ub = Ub,
                Uncertain = u,
                UserUa = item.UaExpression.GetExpressionExecuted(),
                UserUb = item.UbExpression.GetExpressionExecuted(),
                UserU = item.ComplexExpression.GetExpressionExecuted()
            };
            quantities.Add(item, data);
            // 记录错误
            if (!data.Ua.AlmostEqual(data.UserUa))
                quantityErrors.Add(new QuantityError()
                {
                    Title = $"物理量\"{item.Name}\" {item.Symbol} 的A类不确定度计算错误。",
                    Message = $"A类不确定度计算错误。左侧为你输入的公式，右侧为正确的采用逐差法计算A类不确定度的公式，请详细对比。\n可以返回并改正错误。",
                    User = item.UaExpression,
                    Right = Constants.FormulaUncertaintyA
                });
            // B类不确定度还有问题
            if (!data.Ub.AlmostEqual(data.UserUb))
                quantityErrors.Add(new QuantityError()
                {
                    Title = $"物理量\"{item.Name}\" {item.Symbol} 的B类不确定度计算错误。",
                    Message = $"B类不确定度计算错误。左侧为你输入的公式，右侧为正确的公式，请详细对比。\n可以返回并改正错误。",
                    User = item.UbExpression,
                    Right = Constants.FormulaUncertaintyB
                });
            if (!StaticMethods.CalcUncertain(data.Ua, data.Ub).AlmostEqual(data.UserU))
                quantityErrors.Add(new QuantityError()
                {
                    Title = $"物理量\"{item.Name}\" {item.Symbol} 的合成不确定度计算错误。",
                    Message = $"合成不确定度计算错误。左侧为你输入的公式，右侧为正确的公式，请详细对比。\n可以返回并改正错误。",
                    User = item.ComplexExpression,
                    Right = Constants.FormulaUncertainty
                });
        }
    }

    private void DealComplexData()
    {
        CalcArgs calc = new CalcArgs();
        List<CalcArgs.UserInput> input = new List<CalcArgs.UserInput>();
        foreach (var item in quantities)
        {
            calc.AddVariable(item.Key.Symbol, item.Key.UbExpression.GetExpressionExecuted(), item.Key.Groups);
            calc.Measure(item.Key.Symbol, item.Key.Data.ToArray());
            input.Add(new CalcArgs.UserInput()
            {
                name = item.Key.Name,
                u = item.Value.UserU,
                value = item.Value.Average
            });
        }
        var (a, b) = CalcArgs.Calculate(RecordManager.tempRecord.stringExpression, calc);
        var (c, d) = CalcArgs.CalculateValue(a, b, calc);
        var (e, f) = CalcArgs.CalculateValue(a, b, input);
        ComplexAverage = c; ComplexUncertainty = d; UserComplexAverage = e; UserComplexUncertainty = f;
        // 记录错误
        if (!RecordManager.tempRecord.complexQuantityMoedel.AverageExpression.GetExpressionExecuted().AlmostEqual(UserComplexAverage))
            quantityErrors.Add(new QuantityError()
            {
                Title = $"合成量均值计算错误",
                Message = $"合成量均值计算错误。左侧为你输入的公式，右侧为正确的公式，请详细对比。\n可以返回并改正错误。",
                User = RecordManager.tempRecord.complexQuantityMoedel.AverageExpression
            });
        if (!RecordManager.tempRecord.complexQuantityMoedel.UncertainExpression.GetExpressionExecuted().AlmostEqual(UserComplexUncertainty))
            quantityErrors.Add(new QuantityError()
            {
                Title = $"合成量不确定度计算错误",
                Message = $"合成不确定度计算错误。左侧为你输入的公式，右侧为正确的公式，请详细对比。\n可以返回并改正错误。",
                User = RecordManager.tempRecord.complexQuantityMoedel.UncertainExpression
            });
    }

    public void Show()
    {
        DealMeausredQuantities();
        DealComplexData();
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
            if (current.User != null)
            {
                WrongFormula.gameObject.SetActive(true);
                WrongFormula.LoadFormula(current.User);
            }
            else WrongFormula.gameObject.SetActive(false);
            if (current.Right != null)
            {
                RightFormula.gameObject.SetActive(true);
                RightFormula.LoadFormula(current.Right);
            }
            else RightFormula.gameObject.SetActive(false);
            curError++;
        }
        else if (curError == quantityErrors.Count)
        {
            RightFormula.gameObject.SetActive(false);
            WrongFormula.gameObject.SetActive(false);
            Title.text = quantityErrors.Count == 0 ? "正确无误！" : "你已查看了所有的错误！";
            SuccessMessage.text = quantityErrors.Count == 0 ? "你的实验已全部正确地完成！恭喜你掌握了本次误差理论的知识。" :
                "你实验中所有的错误如上，请认真记录并思考。";
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
