using DataBaseModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerAPI.DataBase;
using ServerAPI.DataBase.Repository;
using ServerAPI.Models;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ServerAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataContext dataContext;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext)
        {
            _logger = logger;
            this.dataContext = dataContext;
        }

        public async Task<IActionResult> IndexAsync()
        {
            Repository<SortResult> SortRepository = new(dataContext);
            Repository<PathfindingResults> pathfindingRepository = new(dataContext);


            IEnumerable<string> distinctAlgorithmTypes = await SortRepository
                .GetDistinctAsync(sr => sr.AlgorithmType);
            IEnumerable<string> distinctCollectionType = await SortRepository
                .GetDistinctAsync(sr => sr.CollectionType);
            IEnumerable<string> distinctDataType = await SortRepository
                .GetDistinctAsync(sr => sr.DataType);

            List<string> cpuSortNames = await dataContext.SortResult
                .Where(sr => sr.PC_Config != null && sr.PC_Config.CPU_Config != null)
                .Select(sr => sr.PC_Config.CPU_Config.Name)
                .Distinct()
                .ToListAsync();

            SortResultModel sort = new SortResultModel()
            {
                NameCPU = cpuSortNames,
                AlgorithmType = distinctAlgorithmTypes.ToList(),
                CollectionType = distinctCollectionType.ToList(),
                DataType = distinctDataType.ToList()
            };


            IEnumerable<string> distinctAlgorithmType = await pathfindingRepository
                .GetDistinctAsync(sr => sr.Algotithm);

            List<string> cpuPathfindingNames = await dataContext.PathfindingResults
                .Where(sr => sr.PC_Config != null && sr.PC_Config.CPU_Config != null)
                .Select(sr => sr.PC_Config.CPU_Config.Name)
                .Distinct()
                .ToListAsync();

            PathfindingResultsModel Pathfinding = new PathfindingResultsModel()
            {
                PathfindingNameCPU = cpuPathfindingNames,
                AlgorithmType = distinctAlgorithmType.ToList()
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

            if (sortParam == null ||
                !sortParam.AlgorithmType.Any() ||
                !sortParam.CollectionType.Any() ||
                !sortParam.DataType.Any() ||
                !sortParam.NameCPU.Any())
            {
                return Json(new { message = "At least one field is empty or object is null." });
            }

            Repository<SortResult> SortRepository = new(dataContext);
            Dictionary<string, List<SortResult>> sortsBlocks = new();

            foreach (string algorithm in sortParam.AlgorithmType)
            {
                foreach (string collection in sortParam.CollectionType)
                {
                    foreach (string data in sortParam.DataType)
                    {
                        string chartName = algorithm + " - " + collection + "[" + data + "]";
                        List<SortResult> results = SortRepository
                            .FindAsync(s => s.DataType == data &&
                            s.CollectionType == collection &&
                            s.AlgorithmType == algorithm).Result.ToList();
                        sortsBlocks.Add(chartName, results);
                    }
                }
            }

            int rgbIndex = 4;
            // Створення масиву для осі X
            var labels = new List<int>();

            // Створення списку datasets
            var datasets = new List<object>();

            foreach (KeyValuePair<string, List<SortResult>> entry in sortsBlocks)
            {
                rgbIndex+=3;

                string key = entry.Key;
                List<SortResult> value = entry.Value;

                List<object> data = new List<object>();

                Dictionary<int,List<int>> pairs = new Dictionary<int, List<int>>();

                foreach (SortResult result in value)
                {
                    if(!pairs.ContainsKey(result.Length)) pairs.Add(result.Length, new List<int>());

                    pairs[result.Length].Add(result.Time);
                }

                foreach (KeyValuePair<int, List<int>> result in pairs)
                {
                    if (!labels.Contains(result.Key))
                    {
                        labels.Add(result.Key);
                    }
                    data.Add(new { x = result.Key, y = result.Value.Average() });
                }

                var borderColor = $"rgba({(rgbIndex * 12) % 255}, " +
                    $"{(rgbIndex * 25) % 255}, " +
                    $"{(rgbIndex * 35) % 255}, 1)";
                var backgroundColor = $"rgba({(rgbIndex * 12) % 255}, " +
                    $"{(rgbIndex * 25) % 255}, " +
                    $"{(rgbIndex * 35) % 255}, 0.2)";

                var dataset = new
                {
                    label = key,
                    data = data.ToArray(), // Перетворюємо список у масив
                    borderColor = borderColor,
                    backgroundColor = backgroundColor,
                    fill = false // Без заливки
                };
                datasets.Add(dataset);
            }

            var chartData = new
            {
                labels = labels,  // Числові значення для осі X
                datasets = datasets.ToArray()
            };

            // Повернення даних для графіка
            return Ok(chartData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePathfindingChart([FromBody] PathfindingResultsModel pathfindingParams)
        {
            // Check if the CPU selection is null or empty
            if (pathfindingParams?.PathfindingNameCPU == null || !pathfindingParams.PathfindingNameCPU.Any())
            {
                // If no CPU is selected, return a response without updating the table
                return Json(new { message = "No CPU selected" });
            }

            Repository<PathfindingResults> pathfindingRepository = new(dataContext);

            // Create new data for the table
            var headers = new List<string> { "Algorithm",
                "MinTime",
                "AverageTime",
                "MaxTime",
                "MinLengthPath",
                "AverageLengthPath",
                "MaxLengthPath" };

            var rows = new List<Dictionary<string, string>>();

            foreach (string item in pathfindingParams.AlgorithmType)
            {

                List<PathfindingResults> list = pathfindingRepository
                    .FindAsync(s => s.PC_Config.CPU_Config.Name == pathfindingParams.PathfindingNameCPU.FirstOrDefault()
                    && s.Algotithm == item)
                    .Result.ToList();

                if (list.Count == 0) continue;


                Dictionary<string, string> row = new Dictionary<string, string>();

                string alg = list.First().Algotithm;

                double averageTime = list.Any() ? list.Average(item => item.Time) : 0;
                double maxTime = list.Any() ? list.Max(item => item.Time) : 0;
                double minTime = list.Any() ? list.Min(item => item.Time) : 0;

                double averageLengthPath = list.Any() ? list.Average(item => item.LengthPath) : 0;
                double maxLengthPath = list.Any() ? list.Max(item => item.LengthPath) : 0;
                double minLengthPath = list.Any() ? list.Min(item => item.LengthPath) : 0;


                row.Add("Algorithm", alg);

                row.Add("AverageTime", averageTime.ToString("F3"));
                row.Add("MinTime", minTime.ToString("F3"));
                row.Add("MaxTime", maxTime.ToString("F3"));

                row.Add("AverageLengthPath", averageLengthPath.ToString("F3"));
                row.Add("MinLengthPath", minLengthPath.ToString("F3"));
                row.Add("MaxLengthPath", maxLengthPath.ToString("F3"));

                rows.Add(row);
            }

            // Return the data for the table
            var tableData = new { Headers = headers, Rows = rows };
            return Json(tableData);
        }
    }
}
