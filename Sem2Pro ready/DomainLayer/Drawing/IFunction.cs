using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Drawing
{
    public interface IFunction
    {
        Color Color { get; }
        double LeftBorder { get; }
        double RightBorder { get; }
        string Name { get; }
        System.Drawing.Drawing2D.DashStyle Style { get; }
    }
}
