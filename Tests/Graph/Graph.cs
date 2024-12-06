using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Tests.Graph
{
    public class Graph
    {
        private List<Point2D> vertices { get; set; }    // Список вершин
        private double[,] adjacencyMatrix;    // Матриця суміжності
        // Додаємо властивість Vertices для доступу до всіх вершин
        public List<Point2D> Vertices => vertices;

        // Конструктор, який приймає список вершин
        public Graph(List<Point2D> vertices)
        {
            this.vertices = vertices;
            int vertexCount = vertices.Count;
            adjacencyMatrix = new double[vertexCount, vertexCount];

            // Ініціалізація матриці суміжності значеннями 0 (якщо ребра нема)
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    adjacencyMatrix[i, j] = 0; // Вага 0 за замовчуванням
                }
            }
        }

        // Додавання ребра між двома вершинами
        public void AddEdge(Point2D source, Point2D target)
        {
            int sourceIndex = vertices.IndexOf(source);
            int targetIndex = vertices.IndexOf(target);
            if (sourceIndex == -1 || targetIndex == -1)
            {
                throw new ArgumentException("One or both vertices are not in the graph.");
            }

            // Розрахунок ваги як евклідової відстані між двома точками
            double weight = Math.Sqrt(Math.Pow(target.X - source.X, 2) + Math.Pow(target.Y - source.Y, 2));

            // Запис ваги в матрицю суміжності
            adjacencyMatrix[sourceIndex, targetIndex] = weight;
            adjacencyMatrix[targetIndex, sourceIndex] = weight; // Оскільки граф неспрямований
        }

        // Перевірка наявності ребра між двома вершинами
        public bool EdgeExists(Point2D source, Point2D target)
        {
            int sourceIndex = vertices.IndexOf(source);
            int targetIndex = vertices.IndexOf(target);

            if (sourceIndex == -1 || targetIndex == -1)
            {
                return false; // Одна з вершин не знайдена
            }

            return adjacencyMatrix[sourceIndex, targetIndex] > 0;
        }

        // Отримання ваги ребра між двома вершинами
        public double GetEdgeWeight(Point2D source, Point2D target)
        {
            int sourceIndex = vertices.IndexOf(source);
            int targetIndex = vertices.IndexOf(target);

            if (sourceIndex == -1 || targetIndex == -1)
            {
                throw new ArgumentException("One or both vertices are not in the graph.");
            }

            return adjacencyMatrix[sourceIndex, targetIndex];
        }

        // Виведення всіх вершин
        public void PrintVertices()
        {
            foreach (var vertex in vertices)
            {
                Console.WriteLine(vertex);
            }
        }

        // Виведення всіх ребер
        public void PrintEdges()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = i + 1; j < vertices.Count; j++)
                {
                    if (adjacencyMatrix[i, j] > 0)  // Якщо вага більше 0, то є ребро
                    {
                        Console.WriteLine($"{vertices[i]} -> {vertices[j]} (Weight: {adjacencyMatrix[i, j]:F2})");
                    }
                }
            }
        }
    }

}
