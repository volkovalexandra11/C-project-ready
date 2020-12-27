using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using ZedGraph;
using DataPoint = System.Web.UI.DataVisualization.Charting.DataPoint;

namespace FunctionGraph
{
    public class PointsGetter : IPointsGetter
    {
        public IEnumerable<PointPairList> GetPointForGraph(IEnumerable<Function> functions, double leftBound, double rightBound)
        {
            var a = functions
                .Select(func => GetPointsForOneFunction(func, leftBound, rightBound))
                .ToList();
            return a;
        }

        public PointPairList GetPointsForOneFunction(Function function, double leftBound, double rightBound)
        {
            //const double delta = 0.01;
            leftBound = Math.Max(function.LeftBorder, leftBound);
            rightBound = Math.Min(function.RightBorder, rightBound);
            var curPoint = leftBound;
            //while (curPoint <= rightBound)
            //{
            //    var series = new Series();
            //    while (curPoint <= rightBound + delta && func.FuncBody(curPoint) < 50)
            //    {
            //        series.Points.Add(new DataPoint(curPoint, func.FuncBody(curPoint)));
            //        curPoint += delta;
            //    }

            //    curPoint += delta;
            //    series.Color = func.Color;
            //    series.ChartType = SeriesChartType.FastLine;
            //    series.BorderWidth = 3;
            //    yield return series;
            //}
            var points = new PointPairList();
            const double delta = 0.001;
            for (var x = leftBound; x <= rightBound; x += delta)
            {
                points.Add(new PointPair(x, function.FuncBody(x)));
            }

            return points;
        }
    }
}
