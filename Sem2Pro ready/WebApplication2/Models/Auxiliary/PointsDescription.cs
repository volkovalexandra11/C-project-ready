using System.Collections.Generic;
using System.Drawing;

namespace WebApplication2.Models
{
    public class PointsDescription : IDrawerDescription
    {
        public string Points { get; set; }
        public int LeftBorder { get; set; }
        public int RightBorder { get; set; }
        public Color Color { get; set; }
        public LinesType LineType { get; set; }
    }
}