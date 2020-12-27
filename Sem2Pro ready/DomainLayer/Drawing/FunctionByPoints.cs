using System;
using System.Drawing;
using ZedGraph;

namespace FunctionGraph
{
    public class FunctionByPoints
    {
        public Color Color { get; }
        public double LeftBorder { get; }
        public double RightBorder { get; }
        public string Name { get; }
        public System.Drawing.Drawing2D.DashStyle Style { get; }

        public PointPairList Points { get; }

        public FunctionByPoints(
            Color color,
            string name,
            System.Drawing.Drawing2D.DashStyle style,
            PointPairList points = null,
            double leftBorder = double.MinValue,
            double rightBorder = double.MaxValue
        )
        {
            Color = color;
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Name = name;
            Style = style;
            Points = points;
        }
    }
}