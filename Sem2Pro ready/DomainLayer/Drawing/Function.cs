using System;
using System.Drawing;
using DomainLayer.Drawing;
using ZedGraph;

namespace FunctionGraph
{
    public class Function : IFunction
    {
        public Func<double, double> FuncBody { get; }
        public Color Color { get; }
        public double LeftBorder { get; }
        public double RightBorder { get; }
        public string Name { get; }
        public System.Drawing.Drawing2D.DashStyle Style { get; }


        public Function(Func<double, double> funcBody,
            Color color,
            string name,
            System.Drawing.Drawing2D.DashStyle style,
            double leftBorder = double.MinValue,
            double rightBorder = double.MaxValue
        )
        {
            FuncBody = funcBody;
            Color = color;
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Name = name;
            Style = style;
        }
    }
}
