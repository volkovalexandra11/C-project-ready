using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication2.Models;

namespace UserLayer.Controllers.Auxiliary
{
    public class DrawingService
    {
        private readonly Cache cache;
        private readonly Drawer drawer;
        private readonly Dictionary<LinesType, System.Drawing.Drawing2D.DashStyle> style =
            new Dictionary<LinesType, System.Drawing.Drawing2D.DashStyle>
            {
                [LinesType.Dot] = System.Drawing.Drawing2D.DashStyle.Dot,
                [LinesType.Solid] = System.Drawing.Drawing2D.DashStyle.Solid,
                [LinesType.Dash] = System.Drawing.Drawing2D.DashStyle.Dash,
                [LinesType.DashDot] = System.Drawing.Drawing2D.DashStyle.DashDot
            };

        public DrawingService(Cache cache, Drawer drawer)
        {
            this.cache = cache;
            this.drawer = drawer;
        }

        public Guid Draw(FunctionDescription[] functionDescriptions)
        {
            var name = Guid.NewGuid();
            var functionInfos = functionDescriptions
                .Where(f => f.Function != null)
                .Select(
                    fd => new FunctionInfo(
                        fd.Function,
                        fd.Color,
                        fd.LeftBorder,
                        fd.RightBorder,
                        style[fd.LineType]
                    )
                ).ToArray();
            if (functionInfos.Length == 0)
            {
                throw new DrawingException("Empty input!");
            }
            var picture = drawer.Draw(functionInfos);
            cache.TryAdd(name, picture);
            return name;
        }
    }
}