using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class MeasuredGraphic2 : HTBehaviour
{
    public ScatterChart chart;

    public InputField UserPoint1_X;
    public InputField UserPoint1_y;
    public InputField UserPoint2_X;
    public InputField UserPoint2_y;

    private void Start()
    {
        UserPoint1_y.onValueChanged.AddListener(userponit1listener);
        UserPoint2_y.onValueChanged.AddListener(userponit2listener);
    }


    public void Show(QuantityModel quantity)
    {
        chart.RemoveData();        
        var LineSerie = chart.AddSerie(SerieType.Line);
        var ScatterSerie = chart.AddSerie(SerieType.Scatter);
        var UserScatterSerie = chart.AddSerie(SerieType.Scatter);

        //—˘ Ω…Ë÷√
        ScatterSerie.itemStyle.show = true;
        ScatterSerie.itemStyle.color = Color.red;
        ScatterSerie.symbol.size = 10;
        ScatterSerie.symbol.selectedSize = 15;
        LineSerie.itemStyle.show = true;
        LineSerie.itemStyle.color = Color.black;
        LineSerie.symbol.size = 1;
        LineSerie.symbol.selectedSize = 1;
        UserScatterSerie.itemStyle.show = true;
        UserScatterSerie.itemStyle.color = Color.black;
        UserScatterSerie.symbol.size = 10;
        UserScatterSerie.symbol.selectedSize = 15;


        double[] point_x = new double[quantity.IndependentData.data.Count];
        double[] point_y = new double[quantity.MesuredData.data.Count];
        for (int i = 0; i < quantity.MesuredData.data.Count; i++)
        {
            point_x[i] = double.Parse(quantity.IndependentData.data[i]);
            point_y[i] = double.Parse(quantity.MesuredData.data[i]);
            ScatterSerie.AddXYData(point_x[i], point_y[i]);
        }
        double[] line_x, line_y;
        
        (line_x, line_y) = StaticMethods.MakeLine(point_x, point_y);
        for (int i = 0; i < line_x.Length; i++)
        {
            LineSerie.AddXYData(line_x[i], line_y[i]);
        }

        
    }

    private void userponit1listener(string input)
    {
        double x, y;
        if (UserPoint1_X.text != "")
        {
            x = double.Parse(UserPoint1_X.text);
            y = double.Parse(input);
            chart.series.list[chart.series.list.Count - 1].AddXYData(x, y);
        }
    }

    private void userponit2listener(string input)
    {
        double x, y;
        if (UserPoint2_X.text != "")
        {
            x = double.Parse(UserPoint2_X.text);
            y = double.Parse(input);
            chart.series.list[chart.series.list.Count - 1].AddXYData(x, y);
        }
    }

    public bool CheckAll(bool silent = false)
    {
        return true;
    }
}
