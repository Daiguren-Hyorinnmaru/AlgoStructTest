using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Graph
{
    public interface IPathfindingAlgorithm
    {
        List<Point2D> FindPath(Point2D start, Point2D goal);  // Метод для пошуку шляху
        public void Initialize(Graph graph);
        
    }

}
