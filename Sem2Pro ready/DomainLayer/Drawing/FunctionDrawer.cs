using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DomainLayer.Drawing;

namespace FunctionGraph
{
    public class FunctionDrawer
    {
        private readonly IPointsGetter pointsGetter;
        private readonly IDrawer graphDrawer;

        public FunctionDrawer(IPointsGetter pointsGetter, IDrawer graphDrawer)
        {
            this.pointsGetter = pointsGetter;
            this.graphDrawer = graphDrawer;
        }

        public Bitmap CreateChart(IEnumerable<Function> functions)
        {
            var graphs = functions.Select(f =>
                new Graph(
                    pointsGetter.GetPointsForOneFunction(f, -20, 20), 
                    f.Color, 
                    f.Name,
                    f.Style,
                    f.LeftBorder,
                    f.RightBorder));
            var bitmap = graphDrawer.Draw(graphs);
            return bitmap;
        }

        public Bitmap CreateChart(IEnumerable<FunctionByPoints> functions)
        {
            var graphs = functions.Select(f =>
                new Graph(
                    pointsGetter.GetPointsForOneFunction(f, -20, 20), 
                    f.Color, 
                    f.Name,
                    f.Style,
                    f.LeftBorder,
                    f.RightBorder));
            var bitmap = graphDrawer.Draw(graphs);
            return bitmap;
        }
    }
}