using System.Drawing;

namespace WebApplication2.Models
{
    public class FunctionDescription : IDrawerDescription
    {
        public string Function { get; set; }
        public int LeftBorder { get; set; }
        public int RightBorder { get; set; }
        public LinesType LineType { get; set; }
        public Color Color { get; set; }

        public static FunctionDescription Default = new FunctionDescription
        {
            Function = "",
            LeftBorder = -10,
            RightBorder = 10
        };
    }
}