using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Drawing;
using ZedGraph;

namespace FunctionGraph
{
    public class PointsGetter : IPointsGetter
    {
        public PointPairList GetPointsForOneFunction(Function func, double leftBound, double rightBound)
        {
            leftBound = Math.Max(func.LeftBorder, leftBound);
            rightBound = Math.Min(func.RightBorder, rightBound);
            var points = new PointPairList();
            const double delta = 0.001;
            for (var x = leftBound; x <= rightBound; x += delta)
            {
                var value = func.FuncBody(x);
                if (!double.IsNaN(value))
                    points.Add(new PointPair(x, func.FuncBody(x)));
            }

            return points;
        }

        public PointPairList GetPointsForOneFunction(FunctionByPoints func, double leftBound, double rightBound)
        {
            var points = new PointPairList();
            foreach (var point in func.Points
                .Where(point => !double.IsNaN(point.Y) && (leftBound <= point.X) && (point.X <= rightBound)))
            {
                points.Add(point);
            }

            return points;
        }
    }
}
