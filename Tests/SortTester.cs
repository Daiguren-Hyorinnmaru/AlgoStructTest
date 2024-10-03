using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Tests.Factory;
using Tests.Params;
using Tests.Result;
using Tests.Testing;

namespace Tests
{
    public class SortTester
    {
        private ConcurrentQueue<SortResult> concurrentQueue = new ConcurrentQueue<SortResult>();
        private SortTestFactory sortTestFactory;
        private DataFactory dataFactory;
        private RunTest runTest;

        public SortTester()
        {
            dataFactory = new DataFactory();
            sortTestFactory = new SortTestFactory();
            runTest = new RunTest(sortTestFactory);
            runTest.OnResultAdded += AddConcurrentQueue;
        }

        public void SortTestRun(SortParams sortParams, DataParams dataParams)
        {
            _ = sortTestFactory.CreateSortingActionsAsync(sortParams, dataParams);
        }
        private void AddConcurrentQueue(SortResult sortResult) => concurrentQueue.Enqueue(sortResult);
    }
}
