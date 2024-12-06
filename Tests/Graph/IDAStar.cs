using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Graph
{
    public class IDAStar : IPathfindingAlgorithm
    {
        private Graph graph;
        private Point2D goal;

        public void Initialize(Graph graph)
        {
            this.graph = graph;
        }

        public List<Point2D> FindPath(Point2D start, Point2D goal)
        {
            this.goal = goal;
            double threshold = Heuristic(start, goal);

            while (true)
            {
                var result = Search(start, 0, threshold, new HashSet<Point2D>());
                if (result.Path != null) return result.Path;
                if (result.Cost == double.PositiveInfinity) return null;
                threshold = result.Cost;
            }
        }

        private (List<Point2D> Path, double Cost) Search(Point2D current, double g, double threshold, HashSet<Point2D> visited)
        {
            double f = g + Heuristic(current, goal);
            if (f > threshold) return (null, f);
            if (current.Equals(goal)) return (new List<Point2D> { current }, g);

            visited.Add(current);
            double minCost = double.PositiveInfinity;
            List<Point2D> bestPath = null;

            foreach (var neighbor in GetNeighbors(current))
            {
                if (visited.Contains(neighbor)) continue;

                var result = Search(neighbor, g + graph.GetEdgeWeight(current, neighbor), threshold, visited);

                if (result.Path != null)
                {
                    result.Path.Insert(0, current);
                    return (result.Path, g);
                }

                if (result.Cost < minCost)
                {
                    minCost = result.Cost;
                }
            }

            visited.Remove(current);
            return (null, minCost);
        }

        private double Heuristic(Point2D a, Point2D b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        private List<Point2D> GetNeighbors(Point2D current)
        {
            var neighbors = new List<Point2D>();
            foreach (var vertex in graph.Vertices)
            {
                if (graph.EdgeExists(current, vertex))
                {
                    neighbors.Add(vertex);
                }
            }
            return neighbors;
        }
    }

}
