using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FunctionGraph;
using Infrastructure.TopDowns;
using ZedGraph;

namespace UserLayer.Controllers
{
    public class DrawerForPoints : IFuncDrawer
    {
        private readonly FunctionDrawer fDrawer;

        public DrawerForPoints(FunctionDrawer fDrawer)
        {
            this.fDrawer = fDrawer;
        }

        public byte[] Draw(PointsInfo[] infos)
        {
            var picture = DrawGraph(infos);
            using (var stream = new MemoryStream())
            {
                picture.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }

        private Bitmap DrawGraph(PointsInfo[] infos)
        {
            try
            {
                var funcs = infos.Select(inf =>
                    new FunctionByPoints(inf.Color, inf.Name, inf.Styles, Parse(inf.Function), inf.LeftBorders,
                        inf.RightBorders));

                var picture = fDrawer.CreateChart(funcs.ToArray());
                return picture;
            }
            catch (ParseException e)
            {
                throw new DrawingException(e.Message);
            }
        }

        private static PointPairList Parse(string input)
        {
            input = input
                .Replace("; ", ";")
                .Replace("( ", "(")
                .Replace(" )", ")");

            var splitInp = input.Trim().Split();
            if (splitInp.Any(item => item.EndsWith(";")))
                throw new ParseException("Whitespace between x and y");
            if (!splitInp.All(item => item.StartsWith("(") && item.EndsWith(")")))
                throw new ParseException("Missing bracket");
            var pointPairs = new PointPairList();
            foreach (var pair in splitInp)
            {
                var pointValue = pair.Split(new[] { '(', ')', ';' }, StringSplitOptions.RemoveEmptyEntries);
                var pointPair = new PointPair(double.Parse(pointValue[0]), double.Parse(pointValue[1]));
                pointPairs.Add(pointPair);
            }
            return pointPairs;
        }
    }
}