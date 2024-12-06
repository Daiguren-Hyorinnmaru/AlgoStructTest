using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Graph
{
    public class GreedyBestFirstSearch : IPathfindingAlgorithm
    {
        private Graph graph;

        public void Initialize(Graph graph)
        {
            this.graph = graph;
        }

        public List<Point2D> FindPath(Point2D start, Point2D goal)
        {
            var openList = new SortedSet<Point2D>(Comparer<Point2D>.Create((a, b) =>
                Heuristic(a, goal).CompareTo(Heuristic(b, goal))));
            var cameFrom = new Dictionary<Point2D, Point2D>();
            var visited = new HashSet<Point2D>();

            openList.Add(start);

            while (openList.Count > 0)
            {
                Point2D current = openList.Min;
                openList.Remove(current);

                if (current.Equals(goal))
                {
                    return ReconstructPath(cameFrom, current);
                }

                visited.Add(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (visited.Contains(neighbor)) continue;

                    if (!openList.Contains(neighbor))
                    {
                        cameFrom[neighbor] = current;
                        openList.Add(neighbor);
                    }
                }
            }

            return null; // Шлях не знайдено
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

        private List<Point2D> ReconstructPath(Dictionary<Point2D, Point2D> cameFrom, Point2D current)
        {
            var path = new List<Point2D> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }
    }

}
