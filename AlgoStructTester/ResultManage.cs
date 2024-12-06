using AlgoStructTester.Tab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using Tests;
using Tests.Result;

namespace AlgoStructTester
{
    public class ResultManage
    {
        private SortsControl sortsControl;
        private PathfindingControl pathfindingControl;
        private string URL = "https://localhost:7178/Test/AddResults";

        public ResultManage(SortsControl sortsControl,PathfindingControl pathfindingControl)
        {
            this.sortsControl = sortsControl;
            this.pathfindingControl = pathfindingControl;
            _ = Work();
        }
        private SendResult SetPCParam()
        {
            var sendResult = new SendResult
            {
                RamName = GetRamName(),
                TotalVisibleMemorySize = GetTotalVisibleMemorySize(),
                Speed = GetRamSpeed(),
                CpuName = GetCpuName(),
                NumberOfCores = GetCpuCores(),
                ThreadCount = GetCpuThreads(),
                MaxClockSpeed = GetMaxClockSpeed(),
                SortsResults = new List<SortResult>(),
                PathfindingResults = new List<PathfindingResult>(),
            };

            return sendResult;

            string GetRamName()
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Manufacturer, PartNumber FROM Win32_PhysicalMemory"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        var manufacturer = obj["Manufacturer"]?.ToString() ?? "Unknown Manufacturer";
                        var partNumber = obj["PartNumber"]?.ToString() ?? "Unknown PartNumber";
                        return $"{manufacturer} {partNumber}";
                    }
                }
                return "Unknown RAM";
            }

            int GetTotalVisibleMemorySize()
            {
                using (var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return Convert.ToInt32(obj["TotalVisibleMemorySize"]) / 1024; // Переводимо в MB
                    }
                }
                return 0; // Якщо інформація недоступна
            }

            int GetRamSpeed()
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Speed FROM Win32_PhysicalMemory"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return Convert.ToInt32(obj["Speed"]);
                    }
                }
                return 0; // Якщо інформація недоступна
            }

            string GetCpuName()
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return obj["Name"]?.ToString() ?? "Unknown CPU";
                    }
                }
                return "Unknown CPU";
            }

            int GetCpuCores()
            {
                using (var searcher = new ManagementObjectSearcher("SELECT NumberOfCores FROM Win32_Processor"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return Convert.ToInt32(obj["NumberOfCores"]);
                    }
                }
                return 0;
            }

            int GetCpuThreads()
            {
                using (var searcher = new ManagementObjectSearcher("SELECT ThreadCount FROM Win32_Processor"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return Convert.ToInt32(obj["ThreadCount"]);
                    }
                }
                return 0;
            }

            int GetMaxClockSpeed()
            {
                using (var searcher = new ManagementObjectSearcher("SELECT MaxClockSpeed FROM Win32_Processor"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return Convert.ToInt32(obj["MaxClockSpeed"]);
                    }
                }
                return 0;
            }
        }

        private List<SortResult> SetSortResult()
        {
            List<SortResult> results = new List<SortResult>();
            while (true)
            {
                SortResult t = sortsControl.TakeResult();
                if (t != null)
                {
                    results.Add(t);
                    Task.Delay(5);
                }
                else
                {
                    break;
                }
            }
            return results;
        }
        private List<PathfindingResult> SetPathfindingResult()
        {
            List<PathfindingResult> results = new List<PathfindingResult>();
            while (true)
            {
                PathfindingResult t = pathfindingControl.TakeResult();
                if (t != null)
                {
                    results.Add(t);
                    Task.Delay(5);
                }
                else
                {
                    break;
                }
            }
            return results;
        }

        private async Task SendData()
        {
            Console.WriteLine("start");

            Console.WriteLine("Count " + sortsControl.sortResults.Count);
            SendResult sendResult = SetPCParam();
            sendResult.SortsResults = SetSortResult();
            sendResult.PathfindingResults = SetPathfindingResult();

            Console.WriteLine("Count SortsResults " + sendResult.SortsResults.Count());
            Console.WriteLine("Count PathfindingResults " + sendResult.PathfindingResults.Count());

            try
            {
                // Серіалізація об'єкта в JSON
                Console.WriteLine(JsonSerializer.Serialize(sendResult));
                var json = JsonSerializer.Serialize(sendResult);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Використання HttpClient
                using var client = new HttpClient();
                var response = await client.PostAsync(URL, content);

                // Перевірка відповіді
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Server response: " + result);
                }
                else
                {
                    Console.WriteLine($"An error occurred while sending the request: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }


            Console.WriteLine("Count SortsResults " + sendResult.SortsResults.Count());
            Console.WriteLine("Count PathfindingResults " + sendResult.PathfindingResults.Count());
            Console.WriteLine("end");
        }

        private async Task Work()
        {
            while (true)
            {
                _ = SendData();

                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }
    }
}
