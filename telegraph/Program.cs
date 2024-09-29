using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telegraph
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var coordinates = new List<(int, int)> { (100, 200), (200, 200), (300, 400), (400, 200), (400, 100) };
            Console.WriteLine(FindMinSpanningTree(coordinates)); 

        }
        public static double FindMinSpanningTree(List<(int, int)> coordinates)
        {
            // Создаем граф с ребрами между всеми парами домов
            var graph = new List<(double, int, int)>();
            for (int i = 0; i < coordinates.Count; i++)
            {
                for (int j = i + 1; j < coordinates.Count; j++)
                {
                    var (x1, y1) = coordinates[i];
                    var (x2, y2) = coordinates[j];
                    var distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                    graph.Add((distance, i, j));
                }
            }

            // Сортируем ребра по весу (длине)
            graph.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            // Используем алгоритм Крускала для поиска MST
            var mst = new List<(double, int, int)>();
            var parent = Enumerable.Range(0, coordinates.Count).ToArray();
            var rank = new int[coordinates.Count];

            int Find(int x)
            {
                if (parent[x] != x)
                    parent[x] = Find(parent[x]);
                return parent[x];
            }

            void Union(int x, int y)
            {
                var rootX = Find(x);
                var rootY = Find(y);
                if (rootX != rootY)
                {
                    if (rank[rootX] > rank[rootY])
                        parent[rootY] = rootX;
                    else
                    {
                        parent[rootX] = rootY;
                        if (rank[rootX] == rank[rootY])
                            rank[rootY]++;
                    }
                }
            }

            foreach (var edge in graph)
            {
                var (distance, i, j) = edge;
                if (Find(i) != Find(j))
                {
                    mst.Add(edge);
                    Union(i, j);
                }
            }

            // Вычисляем общую длину MST
            var totalLength = mst.Sum(edge => edge.Item1);

            return (float)totalLength;
        }
    }
}
