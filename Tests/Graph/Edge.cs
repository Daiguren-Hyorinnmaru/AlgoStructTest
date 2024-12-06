using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Graph
{
    public class Edge<T>
    {
        public T Source { get; } // Початкова вершина
        public T Target { get; } // Кінцева вершина
        public double Weight { get; } // Вага ребра

        // Конструктор для створення ребра
        public Edge(T source, T target, Func<T, T, double> calculateWeight)
        {
            Source = source;
            Target = target;
            Weight = calculateWeight(source, target); // Розрахунок ваги під час створення
        }

        // Перевизначення Equals для коректного порівняння
        public override bool Equals(object obj) =>
            obj is Edge<T> other &&
            Equals(Source, other.Source) &&
            Equals(Target, other.Target);

        // Перевизначення GetHashCode для роботи в словниках
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Source?.GetHashCode() ?? 0);
                hash = hash * 23 + (Target?.GetHashCode() ?? 0);
                return hash;
            }
        }

        // Текстове представлення ребра
        public override string ToString() => $"{Source} -> {Target} (Weight: {Weight:F2})";
    }
}
