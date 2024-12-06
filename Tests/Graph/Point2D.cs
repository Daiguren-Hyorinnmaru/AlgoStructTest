using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Graph
{
    public class Point2D
    {
        // Координати вершини
        public double X { get; set; }
        public double Y { get; set; }

        // Конструктор для ініціалізації вершини
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        // Метод для текстового відображення вершини
        public override string ToString() => $"({X}, {Y})";

        // Перевизначення Equals для порівняння вершин
        public override bool Equals(object obj) =>
            obj is Point2D other && X == other.X && Y == other.Y;

        // Перевизначення GetHashCode для використання у словниках і множинах
        public override int GetHashCode()
        {
            unchecked // Дозволяє переповнення чисел для уникнення винятків
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

    }
}
