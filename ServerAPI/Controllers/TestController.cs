using DataBaseModels.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.DataBase;
using ServerAPI.DataBase.Repository;
using ServerAPI.DataBase.UnitOfWork;
using System.Text.Json;
using Tests.Result;
using static System.Formats.Asn1.AsnWriter;
using SortResult = DataBaseModels.Models.SortResult;

namespace ServerAPI.Controllers
{
    public class TestController : Controller
    {
        private readonly DataContext _dataContext;
        public TestController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> AddResults([FromBody] SendResult sendResult)
        {
            PC_ConfigWork pC_ConfigWork = new(_dataContext);
            Repository<SortResult> SortResultRepository = new(_dataContext);
            Repository<PC_Config> PC_ConfigRepository = new(_dataContext);
            Repository<PathfindingResults> PathfindingResultRepository = new(_dataContext);

            CPU_Config cpu_Config = new()
            {
                Name = sendResult.CpuName,
                NumberOfCores = sendResult.NumberOfCores,
                ThreadCount = sendResult.ThreadCount,
                MaxClockSpeed = sendResult.MaxClockSpeed
            };
            RAM_Config ramConfig = new()
            {
                Name = sendResult.RamName,
                TotalVisibleMemorySize = sendResult.TotalVisibleMemorySize,
                Speed = sendResult.Speed
            };

            int idPCConfig =
                pC_ConfigWork.GetIdPC_ConfigAsync(ramConfig, cpu_Config).Result;
            PC_Config config = await PC_ConfigRepository.GetByIdAsync(idPCConfig);
            Console.WriteLine("idPCConfig " + idPCConfig);

            List<SortResult> sortResult = new List<SortResult>();
            int i = 0;
            foreach (Tests.Result.SortResult? sortResultItem in sendResult.SortsResults)
            {
                if (sortResultItem == null) continue;

                SortResult result = new SortResult
                {
                    Time = sortResultItem.Time,
                    Length = sortResultItem.Length,
                    DataType = sortResultItem.DataType.ToString(),
                    AlgorithmType = sortResultItem.Algorithm.ToString(),
                    CollectionType = sortResultItem.Collection.ToString(),
                    PC_Config = config
                };
                Console.WriteLine("sortResultItem " + i++);
                sortResult.Add(result);
            }

            List<PathfindingResults> PathfindingResult = new List<PathfindingResults>();
            foreach (PathfindingResult? PathfindingResultItem in sendResult.PathfindingResults)
            {
                if (PathfindingResultItem == null) continue;

                PathfindingResults result = new PathfindingResults
                {
                    Time = PathfindingResultItem.Time,
                    LengthPath = PathfindingResultItem.LengthPath,
                    Algotithm = PathfindingResultItem.Algotithm.ToString(),
                    PC_Config = config
                };
                Console.WriteLine();
                Console.WriteLine("PathfindingResultItem " + i++);
                Console.WriteLine();
                PathfindingResult.Add(result);
            }
            await PathfindingResultRepository.AddRangeAsync(PathfindingResult);
            await SortResultRepository.AddRangeAsync(sortResult);
            await _dataContext.SaveChangesAsync();
            Console.WriteLine("PathfindingResult "+ PathfindingResult.Count);
            Console.WriteLine("sortResult "+ sortResult.Count);

            return Ok();
        }
    }
}
