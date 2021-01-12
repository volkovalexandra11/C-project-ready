using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
                new Graph(pointsGetter.GetPointsForOneFunction(f, -20, 20), f.Color, f.Name));
            var bitmap = graphDrawer.Draw(graphs);
            return bitmap;
        }
    }
}