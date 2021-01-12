using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FunctionGraph;
using ZedGraph;

namespace DomainLayer.Drawing
{
    public class GraphDrawer : IDrawer
    {
        private readonly ZedGraphControl zedGraph;

        public GraphDrawer(ZedGraphControl control)
        {
            zedGraph = control;
        }
        public Bitmap Draw(IEnumerable<Graph> graphs)
        {
            var gr = graphs.ToList();
            var graphPane = zedGraph.GraphPane;
            graphPane.Title.Text = "";
            graphPane.CurveList.Clear();
            var minY = double.MaxValue;
            var maxY = double.MinValue;
            var minX = gr.Select(graph => graph.LeftBorder).Min();
            var maxX = gr.Select(graph => graph.RightBorder).Max();
            AddFunctions(gr, graphPane, ref minY, ref maxY);
            DrawAxis(graphPane, minX, minY, maxX, maxY);
            graphPane.AxisChange();
            return zedGraph.MasterPane.GetImage(1000, 600, 25);
        }

        private void AddFunctions(List<Graph> gr, GraphPane graphPane, ref double minY, ref double maxY)
        {
            foreach (var graph in gr)
            {
                foreach (var point in graph.Points)
                {
                    if (point.Y < minY)
                        minY = point.Y;
                    if (point.Y > maxY)
                        maxY = point.Y;
                }
                var line = graphPane.AddCurve(graph.Name, graph.Points, graph.Color, SymbolType.None);
                line.Line.Style = graph.Style;
                line.Line.IsSmooth = true;
                line.Line.Width = 3f;
            }
        }

        private void DrawAxis(GraphPane graphPane, double minX, double minY, double maxX, double maxY)
        {
            graphPane.XAxis.Scale.Min = minX - 5;
            graphPane.XAxis.Scale.Max = maxX + 5;
            graphPane.XAxis.Cross = 0.0;
            graphPane.YAxis.Cross = 0.0;
            graphPane.XAxis.Title.Text = "";
            graphPane.YAxis.Title.Text = "";
            graphPane.YAxis.Scale.Max = maxY + 5;
            graphPane.YAxis.Scale.Min = minY - 5;
        }
    }
}
