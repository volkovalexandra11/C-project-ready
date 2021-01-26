using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication2.Models;

namespace UserLayer.Controllers.Auxiliary
{
    public class PointsDrawingService
    {
        private readonly DrawerForPoints fDrawer;
        private readonly Cache cache;
        private readonly Dictionary<LinesType, System.Drawing.Drawing2D.DashStyle> style =
            new Dictionary<LinesType, System.Drawing.Drawing2D.DashStyle>
            {
                [(LinesType)1] = System.Drawing.Drawing2D.DashStyle.Dot,
                [0] = System.Drawing.Drawing2D.DashStyle.Solid,
                [(LinesType)2] = System.Drawing.Drawing2D.DashStyle.Dash,
                [(LinesType)3] = System.Drawing.Drawing2D.DashStyle.DashDot
            };

        public PointsDrawingService(DrawerForPoints fDrawer, Cache cache)
        {
            this.fDrawer = fDrawer;
            this.cache = cache;
        }

        public Guid Draw(PointsDescription[] pointsDescriptions)
        {
            var name = Guid.NewGuid();
            var pointsInfos = pointsDescriptions
                .Where(description => !(description.Points is null))
                .Select((pd, ind) => new PointsInfo(pd.Points, pd.Color, pd.LeftBorder,
                    pd.RightBorder, style[pd.LineType], $"Graph {ind + 1}")).ToArray();
            if (pointsInfos.Length == 0)
            {
                throw new DrawingException("Empty input!");
            }

            var picture = fDrawer.Draw(pointsInfos);
            cache.TryAdd(name, picture);
            return name;
        }
    }
}