using System.Collections.Generic;
using System.Drawing;
using ZedGraph;


namespace FunctionGraph
{
    public class GraphDrawer : IDrawer
    {
        public Bitmap Draw(IEnumerable<Graph> graphs)
        {
            var zedGraph = new ZedGraphControl();
            var graphPane = zedGraph.GraphPane;
            foreach (var graph in graphs)
            {
                var line = graphPane.AddCurve(graph.Name, graph.Points, graph.color, SymbolType.None);
                line.Line.IsSmooth = true;
                line.Line.Width = 3f;
            }

            graphPane.YAxis.Scale.Max = 20;
            graphPane.YAxis.Scale.Min = -20;
            graphPane.AxisChange();
            return zedGraph.MasterPane.GetImage(1000, 600, 25);
        }
    }
}
