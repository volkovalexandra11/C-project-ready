using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

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
            leftBound = Math.Max(function.LeftBorder, leftBound);
            rightBound = Math.Min(function.RightBorder, rightBound);
            var curPoint = leftBound;
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
