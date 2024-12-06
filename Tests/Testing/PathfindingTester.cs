using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tests.Graph;
using Tests.Params;
using Tests.Result;

namespace Tests.Testing
{
    public enum PathfindingAlgotithm
    {
        BellmanFord,
        BFS,
        IDAStar,
        GreedyBestFirstSearch
    }

    public class PathfindingTester
    {
        private ConcurrentQueue<PathfindingResult> concurrentQueue = new ConcurrentQueue<PathfindingResult>();
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public PathfindingTester()
        {

        }

        public async Task PathfindingTestRun(PathfindingParams pathfindingParams)
        {
            await _semaphore.WaitAsync(); // Забезпечує ексклюзивний доступ
            try
            {
                Task _activeTask = Task.Run(() =>
            {
                int length = 10;
                for (int i = 0; i < pathfindingParams.CountIteration; i++)
                {
                    foreach (PathfindingAlgotithm algotithm in pathfindingParams.Algotithm)
                    {
                        Graph.Graph graph = GetGraph(length);

                        IPathfindingAlgorithm pathfinding = GetAlgorithm(algotithm);

                        Point2D start = new Point2D(0, 0);
                        Point2D end = new Point2D(length - 1, length - 1);
                        pathfinding.Initialize(graph);

                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        List<Point2D> path = pathfinding.FindPath(start, end);
                        stopwatch.Stop();

                        double weight = Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));

                        double PathLength = CalculatePathLength(path, graph);

                        //Console.WriteLine(PathLength);
                        //Console.WriteLine(weight);
                        //Console.WriteLine(stopwatch.ElapsedMilliseconds);
                        //Console.WriteLine(algotithm);
                        //Console.WriteLine();

                        PathfindingResult result = new PathfindingResult()
                        {
                            LengthPath = PathLength,
                            Time = (int)stopwatch.ElapsedMilliseconds,
                            Algotithm = algotithm
                        };
                        AddConcurrentQueue(result);
                    }
                }
            });

                await _activeTask;
            }
            finally
            {
                _semaphore.Release(); // Звільняє доступ
            }
        }

        public PathfindingResult TakeResult()
        {
            if (concurrentQueue.TryDequeue(out var result)) return result;
            return null;
        }

        private double CalculatePathLength(List<Point2D> path, Graph.Graph graph)
        {
            if (path == null || path.Count < 2)
            {
                return 0; // Якщо шлях відсутній або складається з однієї вершини
            }

            double totalLength = 0;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Point2D current = path[i];
                Point2D next = path[i + 1];

                // Додаємо вагу ребра між поточними вершинами
                totalLength += graph.GetEdgeWeight(current, next);
            }

            return totalLength;
        }

        private Graph.Graph GetGraph(int length)
        {
            // Створюємо список точок, що утворюють квадрат 30x30
            List<Point2D> vertices = new List<Point2D>();
            int l = length;
            // Заповнюємо список точок (від 0 до 29 по обох осях)
            for (int x = 0; x < l; x++)
            {
                for (int y = 0; y < l; y++)
                {
                    vertices.Add(new Point2D(x, y));
                }
            }

            // Створюємо новий граф з цими точками
            Graph.Graph graph = new Graph.Graph(vertices);
            Math.Sqrt(vertices.Count);
            var rand = new Random();
            for (int x = 1; x < l - 1; x++)
            {
                for (int y = 1; y < l - 1; y++)
                {
                    Point2D current = new Point2D(x, y);

                    int choice = rand.Next(4);

                    switch (choice)
                    {
                        case 0: // Вертикальні ребра (зверху вниз і знизу вгору)
                            {
                                Point2D downNeighbor1 = new Point2D(x, y + 1);
                                Point2D downNeighbor2 = new Point2D(x, y - 1);
                                graph.AddEdge(current, downNeighbor2);
                                graph.AddEdge(current, downNeighbor1);
                                break;
                            }
                        case 1: // Горизонтальні ребра (зліва направо і справа наліво)
                            {
                                Point2D rightNeighbor1 = new Point2D(x + 1, y);
                                Point2D rightNeighbor2 = new Point2D(x - 1, y);
                                graph.AddEdge(current, rightNeighbor1);
                                graph.AddEdge(current, rightNeighbor2);
                                break;
                            }
                        case 2: // Діагональні ребра (вгору-вправо і вниз-вліво)
                            {
                                Point2D downRightNeighbor = new Point2D(x + 1, y + 1);
                                Point2D upLeftNeighbor = new Point2D(x - 1, y - 1);
                                graph.AddEdge(current, downRightNeighbor);
                                graph.AddEdge(current, upLeftNeighbor);
                                break;
                            }
                        case 3: // Діагональні ребра (вниз-вправо і вгору-вліво)
                            {
                                Point2D upRightNeighbor = new Point2D(x + 1, y - 1);
                                Point2D downLeftNeighbor = new Point2D(x - 1, y + 1);
                                //Console.WriteLine(choice);
                                //Console.WriteLine(x);
                                //Console.WriteLine(y);
                                graph.AddEdge(current, upRightNeighbor);
                                graph.AddEdge(current, downLeftNeighbor);
                                break;
                            }
                    }
                }
            }

            for (int x = 1; x < l - 1; x++)
            {
                Point2D current1 = new Point2D(x, 0);
                Point2D current2 = new Point2D(x, l - 1);

                Point2D upRightNeighbor1 = new Point2D(x + 1, 0);
                Point2D downLeftNeighbor1 = new Point2D(x - 1, 0);

                Point2D upRightNeighbor2 = new Point2D(x + 1, l - 1);
                Point2D downLeftNeighbor2 = new Point2D(x - 1, l - 1);

                graph.AddEdge(current1, upRightNeighbor1);
                graph.AddEdge(current1, downLeftNeighbor1);

                graph.AddEdge(current2, upRightNeighbor2);
                graph.AddEdge(current2, downLeftNeighbor2);
            }
            for (int y = 1; y < l - 1; y++)
            {
                Point2D current1 = new Point2D(0, y);
                Point2D current2 = new Point2D(l - 1, y);

                Point2D downNeighbor1 = new Point2D(0, y + 1);
                Point2D upNeighbor1 = new Point2D(0, y - 1);

                Point2D downNeighbor2 = new Point2D(l - 1, y + 1);
                Point2D upNeighbor2 = new Point2D(l - 1, y - 1);

                graph.AddEdge(current1, downNeighbor1);
                graph.AddEdge(current1, upNeighbor1);

                graph.AddEdge(current2, downNeighbor2);
                graph.AddEdge(current2, upNeighbor2);
            }


            return graph;
        }

        private IPathfindingAlgorithm GetAlgorithm(PathfindingAlgotithm pathfindingAlgotithm)
        {
            IPathfindingAlgorithm algorithm = null;

            switch (pathfindingAlgotithm)
            {
                case PathfindingAlgotithm.BellmanFord:
                    {
                        algorithm = new BellmanFord();
                        break;
                    }
                case PathfindingAlgotithm.BFS:
                    {
                        algorithm = new BFS();
                        break;
                    }
                case PathfindingAlgotithm.IDAStar:
                    {
                        algorithm = new IDAStar();
                        break;
                    }
                case PathfindingAlgotithm.GreedyBestFirstSearch:
                    {
                        algorithm = new GreedyBestFirstSearch();
                        break;
                    }
            }

            return algorithm;
        }

        private void AddConcurrentQueue(PathfindingResult Result) => concurrentQueue.Enqueue(Result);
    }
}
