using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Graph
{
    public class BellmanFord : IPathfindingAlgorithm
    {
        private Graph graph;
        private Dictionary<Point2D, double> distances;
        private Dictionary<Point2D, Point2D> cameFrom;

        // Ініціалізація графа
        public void Initialize(Graph graph)
        {
            this.graph = graph;
            distances = new Dictionary<Point2D, double>();
            cameFrom = new Dictionary<Point2D, Point2D>();
        }

        public List<Point2D> FindPath(Point2D start, Point2D goal)
        {
            // Ініціалізація відстаней
            foreach (var vertex in graph.Vertices)
            {
                distances[vertex] = double.PositiveInfinity; // Початкові відстані нескінченні
            }
            distances[start] = 0;

            // Алгоритм Беллмана-Форда: розслаблення ребер (V-1 разів)
            for (int i = 0; i < graph.Vertices.Count - 1; i++)
            {
                foreach (var u in graph.Vertices)
                {
                    foreach (var v in graph.Vertices)
                    {
                        if (graph.EdgeExists(u, v) && distances[u] + graph.GetEdgeWeight(u, v) < distances[v])
                        {
                            distances[v] = distances[u] + graph.GetEdgeWeight(u, v);
                            cameFrom[v] = u;
                        }
                    }
                }
            }

            // Перевірка на цикли від'ємної ваги
            foreach (var u in graph.Vertices)
            {
                foreach (var v in graph.Vertices)
                {
                    if (graph.EdgeExists(u, v) && distances[u] + graph.GetEdgeWeight(u, v) < distances[v])
                    {
                        throw new InvalidOperationException("Graph contains a negative-weight cycle.");
                    }
                }
            }

            // Відновлення шляху
            return ReconstructPath(cameFrom, goal);
        }

        // Відновлення шляху від мети до початку
        private List<Point2D> ReconstructPath(Dictionary<Point2D, Point2D> cameFrom, Point2D current)
        {
            var path = new List<Point2D> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse(); // Відновлюємо шлях від початку до кінця
            return path;
        }
    }

}
