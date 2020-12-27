using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FunctionGraph;
using Infrastructure.TopDowns;

namespace UserLayer.Controllers
{
    public class Drawer 
    {
        private readonly ParserCombinator parser;
        private readonly FunctionDrawer fDrawer;
        
        public Drawer(ParserCombinator parser, FunctionDrawer fDrawer)
        {
            this.parser = parser;
            this.fDrawer = fDrawer;
        }

        public Bitmap DrawGraph(FunctionInfo[] infos)
        {
            try
            {
                var funcs = infos.Select(inf => new Function(parser.Parse(inf.Function), inf.Color, inf.Function,
                    inf.Styles, inf.LeftBorders, inf.RightBorders));
                
                var picture = fDrawer.CreateChart(funcs.ToArray());
                return picture;
            }
            catch (ParseException e)
            {
                throw new DrawingException(e.Message);
            }
        }

        public byte[] Draw(FunctionInfo[] infos)
        {
            var picture = DrawGraph(infos);
            using (var stream = new MemoryStream())
            {
                picture.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
    }
}