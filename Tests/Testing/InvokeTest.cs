using System;
using System.Diagnostics;

namespace Tests.Testing
{
    internal class InvokeTest
    {
        Stopwatch stopwatch;

        public InvokeTest()
        {
            stopwatch = new Stopwatch();
        }

        public void Run(Action action)
        {
            stopwatch.Start();
            action?.Invoke();
            stopwatch.Stop();
        }

        public void Run(Action action, ref long milliseconds)
        {
            stopwatch.Start();
            action?.Invoke();
            stopwatch.Stop();
            milliseconds = stopwatch.ElapsedMilliseconds;
        }

        public void TimeReset() => stopwatch.Reset();

        public long GetTime() => stopwatch.ElapsedTicks;
    }
}
