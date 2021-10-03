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
        double x, y;
        for (int i = 0; i < quantity.MesuredData.data.Count; i++)
        {
            x = double.Parse(quantity.IndependentData.data[i]);
            y = double.Parse(quantity.MesuredData.data[i]);
            ScatterSerie.AddXYData(x, y);
        }
        

        var LineSerie = chart.AddSerie(SerieType.Line);
        x = double.Parse(quantity.IndependentData.data[0]);
        y = double.Parse(quantity.MesuredData.data[0]);
        LineSerie.AddXYData(x, y);
    }

    public bool CheckAll(bool silent = false)
    {
        return true;
    }
}
