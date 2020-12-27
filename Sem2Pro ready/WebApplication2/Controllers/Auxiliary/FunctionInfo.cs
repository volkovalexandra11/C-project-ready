using System.Drawing;
using System.Drawing.Drawing2D;

namespace UserLayer.Controllers
{
    public class FunctionInfo : IInfo
    {
        public string Function { get; }
        public Color Color { get; }
        public int LeftBorders { get; }
        public int RightBorders { get; }
        public DashStyle Styles { get; }

        public FunctionInfo(string function, Color color, int leftBorders, int rightBorders, DashStyle styles)
        {
            Function = function;
            Color = color;
            LeftBorders = leftBorders;
            RightBorders = rightBorders;
            Styles = styles;
        }
    }
}