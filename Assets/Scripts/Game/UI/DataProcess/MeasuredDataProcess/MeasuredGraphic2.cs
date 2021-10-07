using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class MeasuredGraphic2 : HTBehaviour
{
    private QuantityModel quantity;
    private bool ifYSelf;

    public ScatterChart linechart;
    //public ScatterChart pointchart;

    public InputField ChangeRate;
    public InputField UserPoint1_x;
    public InputField UserPoint1_y;
    public InputField UserPoint2_x;
    public InputField UserPoint2_y;

    private void Start()
    {
        UserPoint1_x.onValueChanged.AddListener(userponit1xlistener);
        UserPoint1_y.onValueChanged.AddListener(userponit1ylistener);
        UserPoint2_x.onValueChanged.AddListener(userponit2xlistener);
        UserPoint2_y.onValueChanged.AddListener(userponit2ylistener);
        ChangeRate.onValueChanged.AddListener(input =>
        {
            quantity.change_rate = input;
        });
    }


    public void Show(QuantityModel quantity)
    {
        this.quantity = quantity;

        //pointchart.ClearData();
        linechart.ClearData();
        var LineSerie = linechart.series.list[0];
        var ScatterSerie = linechart.series.list[1];
        var UserScatterSerie = linechart.series.list[2];

        if (quantity.Yaxis == 0)
        {
            //y轴是原变量
            ifYSelf = false;
            linechart.xAxis0.axisName.name = "/" + quantity.selfSymbol;
            linechart.yAxis0.axisName.name = "/" + GameManager.Instance.GetInstrument(quantity.InstrumentType).UnitSymbol;
        }
        else
        {
            ifYSelf = true;
            linechart.xAxis0.axisName.name = "/" + GameManager.Instance.GetInstrument(quantity.InstrumentType).UnitSymbol;
            linechart.yAxis0.axisName.name = "/" + quantity.selfSymbol;
        }


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
            if (ifYSelf)
            {
                LineSerie.AddXYData(double.Parse(line_y[i]), double.Parse(line_x[i]));
            }
            else
            {
                LineSerie.AddXYData(double.Parse(line_x[i]), double.Parse(line_y[i]));
            }
            
        }

        for (int i = 0; i < quantity.MesuredData.data.Count; i++)
        {
            if (ifYSelf)
            {
                ScatterSerie.AddXYData(point_y[i], point_x[i]);
            }
            else
            {
                ScatterSerie.AddXYData(point_x[i], point_y[i]);
            }
            
        }


        if(quantity.point1_x != null && quantity.point1_y != null)
        {
            if (ifYSelf)
            {
                UserPoint1_x.text = quantity.point1_y;
                UserPoint1_y.text = quantity.point1_x;
                linechart.series.list[2].AddXYData(double.Parse(quantity.point1_y), double.Parse(quantity.point1_x));
            }
            else
            {
                UserPoint1_x.text = quantity.point1_x;
                UserPoint1_y.text = quantity.point1_y;
                linechart.series.list[2].AddXYData(double.Parse(quantity.point1_x), double.Parse(quantity.point1_y));
            }           
        }
        if (quantity.point2_x != null && quantity.point2_y != null)
        {
            if (ifYSelf)
            {
                UserPoint2_x.text = quantity.point2_y;
                UserPoint2_y.text = quantity.point2_x;
                linechart.series.list[2].AddXYData(double.Parse(quantity.point2_y), double.Parse(quantity.point2_x));
            }
            else
            {
                UserPoint2_x.text = quantity.point2_x;
                UserPoint2_y.text = quantity.point2_y;
                linechart.series.list[2].AddXYData(double.Parse(quantity.point2_x), double.Parse(quantity.point2_y));
            }           
        }

        if (quantity.change_rate != null)
        {
            ChangeRate.text = quantity.change_rate;
        }
    }

    private void userponit1xlistener(string input)
    {
        quantity.point1_x = input;
        if (quantity.point1_y != null)
        {
            double x = double.Parse(quantity.point1_x);
            double y = double.Parse(quantity.point1_y);
            linechart.series.list[2].ClearData();
            if (ifYSelf)
            {
                linechart.series.list[2].AddXYData(y, x);
            }
            else
            {
                linechart.series.list[2].AddXYData(x, y);
            }
            
            if(quantity.point2_x != null && quantity.point2_y != null)
            {
                x = double.Parse(quantity.point2_x);
                y = double.Parse(quantity.point2_y);
                if (ifYSelf)
                {
                    linechart.series.list[2].AddXYData(y, x);
                }
                else
                {
                    linechart.series.list[2].AddXYData(x, y);
                }
            }
        }
    }
    private void userponit1ylistener(string input)
    {
        quantity.point1_y = input;
        if (quantity.point1_x != null)
        {
            double x = double.Parse(quantity.point1_x);
            double y = double.Parse(quantity.point1_y);
            linechart.series.list[2].ClearData();
            if (ifYSelf)
            {
                linechart.series.list[2].AddXYData(y, x);
            }
            else
            {
                linechart.series.list[2].AddXYData(x, y);
            }

            if (quantity.point2_x != null && quantity.point2_y != null)
            {
                x = double.Parse(quantity.point2_x);
                y = double.Parse(quantity.point2_y);
                if (ifYSelf)
                {
                    linechart.series.list[2].AddXYData(y, x);
                }
                else
                {
                    linechart.series.list[2].AddXYData(x, y);
                }
            }
        }
    }
    private void userponit2xlistener(string input)
    {
        quantity.point2_x = input;
        if (quantity.point2_y != null)
        {
            double x = double.Parse(quantity.point2_x);
            double y = double.Parse(quantity.point2_y);
            linechart.series.list[2].ClearData();
            if (ifYSelf)
            {
                linechart.series.list[2].AddXYData(y, x);
            }
            else
            {
                linechart.series.list[2].AddXYData(x, y);
            }

            if (quantity.point1_x != null && quantity.point1_y != null)
            {
                x = double.Parse(quantity.point1_x);
                y = double.Parse(quantity.point1_y);
                if (ifYSelf)
                {
                    linechart.series.list[2].AddXYData(y, x);
                }
                else
                {
                    linechart.series.list[2].AddXYData(x, y);
                }
            }
        }
    }
    private void userponit2ylistener(string input)
    {
        quantity.point2_y = input;
        if (quantity.point2_x != null)
        {
            double x = double.Parse(quantity.point2_x);
            double y = double.Parse(quantity.point2_y);
            linechart.series.list[2].ClearData();
            if (ifYSelf)
            {
                linechart.series.list[2].AddXYData(y, x);
            }
            else
            {
                linechart.series.list[2].AddXYData(x, y);
            }

            if (quantity.point1_x != null && quantity.point1_y != null)
            {
                x = double.Parse(quantity.point1_x);
                y = double.Parse(quantity.point1_y);
                if (ifYSelf)
                {
                    linechart.series.list[2].AddXYData(y, x);
                }
                else
                {
                    linechart.series.list[2].AddXYData(x, y);
                }
            }
        }
    }

    public bool CheckAll(bool silent = false)
    {
        return true;
    }
}
