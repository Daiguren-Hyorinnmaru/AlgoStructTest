using Microsoft.AspNetCore.Mvc;
using ServerAPI.Models;
using System.Diagnostics;

namespace ServerAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            SortResultModel sort = new SortResultModel()
            {
                AlgorithmType = new List<string>() { "Algorithm1", "Algorithm2", "Algorithm3" },
                CollectionType = new List<string>() { "Collection1", "Collection2", "Collection3" },
                DataType = new List<string>() { "Data1", "Data2", "Data3" }
            };
            PathfindingResultsModel Pathfinding = new PathfindingResultsModel()
            {
                AlgorithmType = new List<string>() { "Algorithm1", "Algorithm2", "Algorithm3" }
            };

            IndexModel model = new IndexModel()
            {
                Sort = sort,
                Pathfinding = Pathfinding
            };

            return View(model);  // передача моделі в представлення
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSortChart([FromBody] SortResultModel sortParam)
        {
            if (sortParam != null)
            {
                Console.WriteLine("AlgorithmType:");
                if (sortParam.AlgorithmType != null)
                    sortParam.AlgorithmType.ForEach(Console.WriteLine);
                else
                    Console.WriteLine("No AlgorithmType data received.");

                Console.WriteLine("CollectionType:");
                if (sortParam.CollectionType != null)
                    sortParam.CollectionType.ForEach(Console.WriteLine);
                else
                    Console.WriteLine("No CollectionType data received.");

                Console.WriteLine("DataType:");
                if (sortParam.DataType != null)
                    sortParam.DataType.ForEach(Console.WriteLine);
                else
                    Console.WriteLine("No DataType data received.");
            }
            else
            {
                Console.WriteLine("sortParam is null");
            }

            // Тут можемо виконати обчислення або підготувати дані для графіка
            // Наприклад, формуємо дані для графіка
            var chartData = new
            {
                labels = new[] { 1, 2, 3, 4, 5, 6 }, // Числові значення для осі X
                datasets = Enumerable.Range(1, 20).Select(index => new
                {
                    label = $"Лінія {index}",
                    data = new[]
                    {
            new { x = 1, y = 12 + index },
            new { x = 2, y = 15 + index },
            new { x = 3, y = 8 + index },
            new { x = 4, y = 6 + index },
            new { x = 5, y = 9 + index },
            new { x = 6, y = 4 + index }
        },
                    borderColor = $"rgba({(index * 12) % 255}, {(index * 25) % 255}, {(index * 35) % 255}, 1)",
                    backgroundColor = $"rgba({(index * 12) % 255}, {(index * 25) % 255}, {(index * 35) % 255}, 0.2)",
                    fill = false // Без заливки
                }).ToArray()
            };

            // Повернення даних для графіка
            return Ok(chartData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePathfindingChart([FromBody] PathfindingResultsModel pathfindingParam)
        {

            if (pathfindingParam != null)
            {
                Console.WriteLine("AlgorithmType:");
                if (pathfindingParam.AlgorithmType != null)
                    pathfindingParam.AlgorithmType.ForEach(Console.WriteLine);
                else
                    Console.WriteLine("No AlgorithmType data received.");
            }
            else
            {
                Console.WriteLine("sortParam is null");
            }

            var chartData = new
            {
                labels = new[] { 1, 2, 3, 4, 5, 6 }, // Числові значення для осі X
                datasets = new[]
                {
            new
            {
                label = "Лінія 1",
                data = new[]
                {
                    new { x = 1, y = 12 },
                    new { x = 2, y = 15 },
                    new { x = 3, y = 8 },
                    new { x = 4, y = 6 },
                    new { x = 5, y = 9 },
                    new { x = 6, y = 4 }
                },
                borderColor = "rgba(75, 192, 192, 1)",
                backgroundColor = "rgba(75, 192, 192, 0.2)",
                fill = false // Без заливки
            },
            new
            {
                label = "Лінія 2",
                data = new[]
                {
                    new { x = 1, y = 5 },
                    new { x = 2, y = 1 },
                    new { x = 3, y = 14 },
                    new { x = 4, y = 18 },
                    new { x = 5, y = 4 },
                    new { x = 6, y = 11 }
                },
                borderColor = "rgba(153, 102, 255, 1)",
                backgroundColor = "rgba(153, 102, 255, 0.2)",
                fill = false // Без заливки
            }
        }
            };

            return Ok(chartData);
        }

    }
}
