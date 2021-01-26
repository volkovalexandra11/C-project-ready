using System.Drawing;
using System.Drawing.Drawing2D;
using ZedGraph;

namespace FunctionGraph
{
    public class Graph
    {
        public PointPairList Points { get; }
        public Color Color { get; }
        public string Name { get; }
        public DashStyle Style { get; }
        public double LeftBorder { get; }
        public double RightBorder { get; }

        public Graph(PointPairList points, Color color, string name, DashStyle style, double leftBorder, double rightBorder)
        {
            Points = points;
            Color = color;
            Name = name;
            Style = style;
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
        }
    }
}