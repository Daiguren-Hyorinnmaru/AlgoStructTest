using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Graph
{
    public class BFS : IPathfindingAlgorithm
    {
        private Graph graph;
        private Dictionary<Point2D, Point2D> cameFrom;
        private HashSet<Point2D> visited;

        // Ініціалізація графа
        public void Initialize(Graph graph)
        {
            this.graph = graph;
            cameFrom = new Dictionary<Point2D, Point2D>();
            visited = new HashSet<Point2D>();
        }

        public List<Point2D> FindPath(Point2D start, Point2D goal)
        {
            // Черга для зберігання вершин, які потребують обробки
            var queue = new Queue<Point2D>();
            queue.Enqueue(start);

            // Позначаємо стартову точку як відвідану
            visited.Add(start);

            while (queue.Count > 0)
            {
                // Беремо з черги наступну вершину
                Point2D current = queue.Dequeue();

                // Якщо знайшли мету, відновлюємо шлях
                if (current.Equals(goal))
                {
                    return ReconstructPath(cameFrom, current);
                }

                // Перевіряємо всі сусідні вершини
                foreach (var neighbor in GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        cameFrom[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            // Якщо шляху не знайдено
            return null;
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

        // Отримання сусідів поточної вершини
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
