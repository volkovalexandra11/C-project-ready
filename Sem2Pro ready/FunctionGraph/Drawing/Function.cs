using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FunctionGraph
{
    public class Function
    {
        public Function(Func<double, double> funcBody,
            Color color,
            string name,
            double leftBorder = double.MinValue,
            double rightBorder = double.MaxValue
        )
        {
            FuncBody = funcBody;
            Color = color;
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Name = name;
        }

        public Func<double, double> FuncBody { get; }
        public Color Color { get; }
        public double LeftBorder { get; }
        public double RightBorder { get; }
        public string Name { get; }
    }
}
