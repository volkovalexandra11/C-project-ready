using System.Drawing;
using System.Drawing.Drawing2D;
using ZedGraph;

namespace FunctionGraph
{
    public class Graph
    {
        public PointPairList Points { get; }
        public readonly Color color;
        public string Name { get; }
        public DashStyle style { get; }
        public double LeftBorder { get; }
        public double RightBorder { get; }

        public Graph(PointPairList points, Color color, string name, DashStyle style, double leftBorder, double rightBorder)
        {
            Points = points;
            this.color = color;
            Name = name;
            this.style = style;
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
        }
    }
}