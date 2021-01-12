using System.Drawing;
using ZedGraph;

namespace FunctionGraph
{
    public class Graph
    {
        public readonly PointPairList Points;
        public readonly Color color;
        public string Name { get; }

        public Graph(PointPairList points, Color color, string name)
        {
            Points = points;
            this.color = color;
            Name = name;
        }
    }
}