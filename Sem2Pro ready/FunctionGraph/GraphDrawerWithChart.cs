using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web.UI.DataVisualization.Charting;

namespace FunctionGraph
{
    public class GraphDrawerWithChart  //:IDrawer
    {
        public Chart Draw(double leftBound, double rightBound, IEnumerable<Series> funcSeries)
        {
            var chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());
            chart.ChartAreas[0].AxisX.Minimum = leftBound;
            chart.ChartAreas[0].AxisX.Maximum = rightBound;
            chart.ChartAreas[0].AxisX.Interval = 10;
            foreach(var series in funcSeries)
            {
                chart.Series.Add(series);
            }

            var max = chart.Series
                .SelectMany(ser => ser.Points)
                .SelectMany(point => point.YValues)
                .Max();

            var min = chart.Series
                .SelectMany(ser => ser.Points)
                .SelectMany(point => point.YValues)
                .Min();

            chart.ChartAreas[0].AxisY.Minimum = -20;
            chart.ChartAreas[0].AxisY.Maximum = 20;
            chart.ChartAreas[0].AxisY.Interval = (-chart.ChartAreas[0].AxisY.Minimum + chart.ChartAreas[0].AxisY.Maximum) / 10;
            return chart;
        }
    }
}
