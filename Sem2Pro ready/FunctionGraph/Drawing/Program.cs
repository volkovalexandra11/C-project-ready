using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using Ninject;
using Ninject.Extensions.Factory;
using Ninject.Extensions.Conventions;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;

namespace FunctionGraph
{
    public class Program
    { 
        static void Main()
        {
            double Func(double x) => x + 2;
            var function = new Function(Func, Color.Coral, "x+2");
            var functionSquare = new Function(x => x * x, Color.Green, "x^2");
            var complexFunc = new Function(Math.Tan, Color.Blue, "tg(x)");
            var sinFunc = new Function(Math.Sin, Color.Yellow, "sin(x)");
            var cosFunc = new Function(Math.Cos, Color.Orchid, "cos(x)");
            var functions = new[] {sinFunc, cosFunc, functionSquare, complexFunc};
            var a = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            var b = a.Replace('/', ' ').Replace(':', ' ');
            //var Points = new PointsGetter().GetPointForGraph(functions, -10, 10);
            var picture = new FunctionDrawer(new PointsGetter(), new GraphDrawer()).CreateChart(functions);
            picture.Save($"test {b}.jpeg", ImageFormat.Jpeg);
        }
    }
}
