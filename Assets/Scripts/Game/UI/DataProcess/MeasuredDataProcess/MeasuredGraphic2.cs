using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class MeasuredGraphic2 : HTBehaviour
{
    public ScatterChart linechart;
    public ScatterChart pointchart;

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
        linechart.ClearData();
        pointchart.ClearData();
        var LineSerie = linechart.series.list[0];
        var ScatterSerie = pointchart.series.list[0];
        var UserScatterSerie = pointchart.series.list[1];


        double[] point_x = new double[quantity.IndependentData.data.Count];
        double[] point_y = new double[quantity.MesuredData.data.Count];
        for (int i = 0; i < quantity.MesuredData.data.Count; i++)
        {
            point_x[i] = double.Parse(quantity.IndependentData.data[i]);
            point_y[i] = double.Parse(quantity.MesuredData.data[i]);
        }
        string[] line_x, line_y;

        (line_x, line_y) = StaticMethods.MakeLine(point_x, point_y);
        for (int i = 0; i < line_x.Length; i++)
        {
            LineSerie.AddXYData(double.Parse(line_x[i]), double.Parse(line_y[i]));
        }

        pointchart.xAxis0.min = linechart.xAxis0.min;
        pointchart.xAxis0.max = linechart.xAxis0.max;
        pointchart.yAxis0.min = linechart.yAxis0.min;
        pointchart.yAxis0.max = linechart.yAxis0.max;

        for (int i = 0; i < quantity.MesuredData.data.Count; i++)
        {
            ScatterSerie.AddXYData(point_x[i], point_y[i]);
        }

    }

    private void userponit1listener(string input)
    {
        double x, y;
        if (UserPoint1_X.text != "")
        {
            x = double.Parse(UserPoint1_X.text);
            y = double.Parse(input);
            pointchart.series.list[1].AddXYData(x, y);
        }
    }

    private void userponit2listener(string input)
    {
        double x, y;
        if (UserPoint2_X.text != "")
        {
            x = double.Parse(UserPoint2_X.text);
            y = double.Parse(input);
            pointchart.series.list[1].AddXYData(x, y);
        }
    }

    public bool CheckAll(bool silent = false)
    {
        return true;
    }
}
