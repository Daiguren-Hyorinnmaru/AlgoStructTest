using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tests.Factory;
using Tests.Result;

namespace Tests.Testing
{
    internal class RunTest
    {
        private readonly SortTestFactory _sortTestFactory;

        // Concurrent queue to store sorting actions and results
        private readonly ConcurrentQueue<(SortResult, Action)> _actionQueue;

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        public event Action<SortResult> OnResultAdded;

        public RunTest(SortTestFactory sortTestFactory)
        {
            _actionQueue = new ConcurrentQueue<(SortResult, Action)>();
            _sortTestFactory = sortTestFactory;

            // Subscribe to the ResultAdded event from the SortTestFactory
            _sortTestFactory.ResultAdded += EnqueueActions;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            // Start processing the queue asynchronously
            _ = Task.Run(() => ProcessActionQueueAsync(_cancellationToken));
            _ = StartPeriodicMemoryCleanupAsync();
        }

        // Asynchronous method to process actions from the queue
        private async Task ProcessActionQueueAsync(CancellationToken token)
        {
            InvokeTest invokeTest = new InvokeTest(); // Instance to run the sorting tests

            // Continuously process until cancellation is requested
            while (!token.IsCancellationRequested)
            {
                // Try to dequeue an action from the queue
                if (_actionQueue.TryDequeue(out var actionTuple))
                {
                    var (sortResult, action) = actionTuple;

                    invokeTest.TimeReset();
                    invokeTest.Run(action);
                    sortResult.Time = invokeTest.GetTime();

                    // Notify that a result has been added
                    OnResultAdded?.Invoke(sortResult);
                }
                else
                {
                    // If no actions are available, wait for a short period
                    await Task.Delay(250, token);
                }
            }
        }

        // Method to add actions to the queue when sorting results are available
        private void EnqueueActions(List<(SortResult, Action)> actionTuple)
        {
            foreach (var item in actionTuple)
            {
                _actionQueue.Enqueue(item);
            }
        }

        public static async Task StartPeriodicMemoryCleanupAsync()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                });

                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }
    }

}
