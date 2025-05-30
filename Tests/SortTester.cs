﻿using System.Collections.Concurrent;
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
        private RunSortTest runTest;

        public SortTester()
        {
            dataFactory = new DataFactory();
            sortTestFactory = new SortTestFactory();
            runTest = new RunSortTest(sortTestFactory);
            runTest.OnResultAdded += AddConcurrentQueue;
        }

        public async void SortTestRun(SortParams sortParams, DataParams dataParams)
        {
            await sortTestFactory.CreateSortingActionsAsync(sortParams, dataParams);
        }

        public SortResult TakeResult()
        {
            if (concurrentQueue.TryDequeue(out var result)) return result;
            return null;
        }

        private void AddConcurrentQueue(SortResult sortResult) => concurrentQueue.Enqueue(sortResult);
    }
}
