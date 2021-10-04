using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts;

public class MeasuredGraphic2 : HTBehaviour
{
    public ScatterChart chart;

    public void Show(QuantityModel quantity)
    {
        chart.RemoveData();
        var ScatterSerie = chart.AddSerie(SerieType.Scatter);
        double[] point_x = new double[quantity.IndependentData.data.Count];
        double[] point_y = new double[quantity.MesuredData.data.Count];
        for (int i = 0; i < quantity.MesuredData.data.Count; i++)
        {
            point_x[i] = double.Parse(quantity.IndependentData.data[i]);
            point_y[i] = double.Parse(quantity.MesuredData.data[i]);
            ScatterSerie.AddXYData(point_x[i], point_y[i]);
        }
        
        var LineSerie = chart.AddSerie(SerieType.Line);
        double[] line_x, line_y;
        
        (line_x, line_y) = StaticMethods.MakeLine(point_x, point_y);
        for (int i = 0; i < line_x.Length; i++)
        {
            LineSerie.AddXYData(point_x[i], point_y[i]);
        }

    }

    public bool CheckAll(bool silent = false)
    {
        return true;
    }
}
