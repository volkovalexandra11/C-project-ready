using System.Drawing;
using System.Drawing.Drawing2D;

namespace UserLayer.Controllers
{
    public class PointsInfo : IInfo
    {
        public string Function { get; }
        public Color Color { get; }
        public int LeftBorders { get; }
        public int RightBorders { get; }
        public DashStyle Styles { get; }
        public string Name { get; }

        public PointsInfo(string function, Color color, int leftBorders, int rightBorders, DashStyle styles, string name)
        {
            Color = color;
            LeftBorders = leftBorders;
            RightBorders = rightBorders;
            Styles = styles;
            Name = name;
            Function = function;
        }
    }
}