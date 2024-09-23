using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class RunTest
    {
        Stopwatch stopwatch;

        public RunTest()
        {
            stopwatch = new Stopwatch();
        }

        public RunTest(Action action)
        {
            stopwatch.Start();
            action?.Invoke();
            stopwatch.Stop();
        }

        public void TimeRest() => stopwatch.Reset();

        public long GetTime() => stopwatch.ElapsedTicks;
    }
}
