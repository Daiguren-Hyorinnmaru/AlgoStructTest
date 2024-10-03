using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tests.Algorithms;
using Tests.Params;
using Tests.Result;

namespace Tests.Factory
{
    internal class SortTestFactory
    {
        private SortParams _sortParameters;
        private CollectionFactory _collectionFactory;

        private Task _activeTask;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly object _lock = new object();

        // Event to notify when sorting results are available
        public event Action<List<(SortResult, Action)>> ResultAdded;

        public SortTestFactory()
        {
            _collectionFactory = new CollectionFactory(new DataFactory());
        }

        // Asynchronous method to create sorting actions based on SortParams
        public async Task CreateSortingActionsAsync(SortParams sortParams, DataParams dataParams)
        {
            lock (_lock)
            {
                // Cancel any ongoing task before starting a new one
                CancelCurrentTask();

                _cancellationTokenSource = new CancellationTokenSource();
                _sortParameters = sortParams;
                _collectionFactory.SetDataParams(dataParams);

                _activeTask = Task.Run(() => ExecuteCreateActions(_cancellationTokenSource.Token));
            }

            await _activeTask;
        }

        // Method to cancel the current task if it's running
        private void CancelCurrentTask()
        {
            if (_activeTask != null && !_activeTask.IsCompleted)
            {
                _cancellationTokenSource?.Cancel();
                try
                {
                    _activeTask.Wait();
                }
                catch (AggregateException) { }
            }
        }

        // Method to execute the creation of sorting actions
        private void ExecuteCreateActions(CancellationToken cancellationToken)
        {
            for (int length = _sortParameters.LengthStart; length <= _sortParameters.LengthEnd; length += _sortParameters.Step)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var sortResults = GenerateSortResults(length);
                var list = new List<(SortResult, Action)>();

                // Iterate through each generated sort result
                foreach (var sortResult in sortResults)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    var collection = _collectionFactory.functions[sortResult.DataType](sortResult.Collection, length);
                    Action action = SortingAlgorithms.GetActionAlgorithm(sortResult.Algorithm, collection);
                    list.Add((sortResult, action));
                }

                // Invoke the event to notify that results have been added
                ResultAdded?.Invoke(list);
            }
        }

        // Method to generate sort results based on specified parameters
        private IEnumerable<SortResult> GenerateSortResults(int length)
        {
            return _sortParameters.DataTypes.SelectMany(dataType =>
                _sortParameters.Algorithms.SelectMany(algType =>
                    _sortParameters.Collections.Select(colType => new SortResult
                    {
                        Algorithm = algType,
                        Collection = colType,
                        Length = length,
                        DataType = dataType
                    })));
        }
    }
}
