using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AlgoStructTester
{
    internal class PCConfig
    {

        public PCConfig()
        {
            {
                // Get all possible CPU parameters
                Console.WriteLine("CPU Information:");
                var cpuSearcher = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (var obj in cpuSearcher.Get())
                {
                    foreach (var property in obj.Properties)
                    {
                        //Console.WriteLine($"{property.Name}: {property.Value}");
                    }
                    Console.WriteLine();
                }

                // Get general RAM information
                Console.WriteLine("Computer System Information:");
                var systemSearcher = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
                foreach (var obj in systemSearcher.Get())
                {
                    foreach (var property in obj.Properties)
                    {
                        //Console.WriteLine($"{property.Name}: {property.Value}");
                    }
                    Console.WriteLine();
                }

                // Get detailed information for each RAM module
                Console.WriteLine("Physical Memory (RAM) Modules Information:");
                var ramSearcher = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
                foreach (var obj in ramSearcher.Get())
                {
                    foreach (var property in obj.Properties)
                    {
                        //Console.WriteLine($"{property.Name}: {property.Value}");
                    }
                    Console.WriteLine();
                }

                // Get overall RAM information
                Console.WriteLine("Overall RAM Information:");
                var osSearcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                foreach (var obj in osSearcher.Get())
                {
                    foreach (var property in obj.Properties)
                    {
                        //Console.WriteLine($"{property.Name}: {property.Value}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
